using System;
using System.Buffers.Binary;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Utilities.Threading;
using Microsoft.Extensions.Logging;
using Qommon;
using Qommon.Threading;
#if NET6_0_OR_GREATER
using System.IO.Compression;
#endif

namespace Disqord.WebSocket.Default.Discord
{
    internal sealed class DiscordWebSocket : IAsyncDisposable
    {
        public const int ReceiveBufferSize = 8192;

        public ILogger Logger { get; }

        private readonly IWebSocketClientFactory _webSocketClientFactory;
        private readonly bool _supportsZLib;

        private IWebSocketClient? _ws;

        /// <summary>
        ///     This is a fix for the ClientWebSocket being garbage and aborting itself on a cancelled ReceiveAsync (and possibly SendAsync)
        ///     rendering us unable to close the connection gracefully.
        ///     1. We create an infinite task that runs alongside the Send/ReceiveAsync() tasks and pass it the actual cancellation token just to signal cancellation.
        ///     2. We pass the Send/ReceiveAsync() task an essentially bogus cancellation token that gets cancelled when we close the connection,
        ///        allowing us to gracefully close and then have the websocket abort or whatever as we don't care about the state of it anymore.
        /// </summary>
        private Cts? _limboCts;

        private readonly SemaphoreSlim _sendSemaphore;

        private readonly SemaphoreSlim _receiveSemaphore;
        private readonly byte[] _receiveBuffer;
        private readonly MemoryStream _receiveStream;
        private Stream? _receiveZLibStream;

        // Used to ensure the ZLib suffix was read after deserialization.
        private bool _wasLastPayloadZLib;

        private bool _isDisposed;

        public DiscordWebSocket(
            ILogger logger,
            IWebSocketClientFactory webSocketClientFactory,
            bool supportsZLib = true)
        {
            Logger = logger;
            _webSocketClientFactory = webSocketClientFactory;
            _supportsZLib = supportsZLib;

            _sendSemaphore = new SemaphoreSlim(1, 1);

            _receiveSemaphore = new SemaphoreSlim(1, 1);
            _receiveBuffer = new byte[ReceiveBufferSize];
            _receiveStream = new MemoryStream(ReceiveBufferSize * 2);
        }

        private void ThrowIfDisposed()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(null, "The Discord web socket client has been disposed.");
        }

        public async ValueTask ConnectAsync(Uri url, CancellationToken cancellationToken)
        {
            using (await _sendSemaphore.EnterAsync(cancellationToken).ConfigureAwait(false))
            using (await _receiveSemaphore.EnterAsync(cancellationToken).ConfigureAwait(false))
            {
                ThrowIfDisposed();

                _limboCts?.Cancel();
                _limboCts?.Dispose();
                _limboCts = new Cts();
                _ws?.Dispose();
                _ws = _webSocketClientFactory.CreateClient();
                if (_supportsZLib)
                {
                    _wasLastPayloadZLib = false;
                    _receiveZLibStream?.Dispose();
                    _receiveZLibStream = new ZLibStream(_receiveStream, CompressionMode.Decompress, true);
                }

                await _ws.ConnectAsync(url, cancellationToken).ConfigureAwait(false);
            }
        }

        public async ValueTask SendAsync(Memory<byte> memory, CancellationToken cancellationToken)
        {
            using (await _sendSemaphore.EnterAsync(cancellationToken).ConfigureAwait(false))
            {
                ThrowIfDisposed();

                // See _limboCts for more info on cancellation.
                var sendTask = _ws!.SendAsync(memory, WebSocketMessageType.Text, true, _limboCts!.Token).AsTask();
                using (var infiniteCts = Cts.Linked(cancellationToken))
                {
                    var infiniteTask = Task.Delay(Timeout.Infinite, infiniteCts.Token);
                    await Task.WhenAny(infiniteTask, sendTask).ConfigureAwait(false);
                    infiniteCts.Cancel();
                }

                cancellationToken.ThrowIfCancellationRequested();
            }
        }

        public async ValueTask<Stream> ReceiveAsync(CancellationToken cancellationToken)
        {
            using (await _receiveSemaphore.EnterAsync(cancellationToken).ConfigureAwait(false))
            {
                ThrowIfDisposed();

                Guard.IsNotNull(_ws);
                Guard.IsNotNull(_limboCts);

                // Ensures that the receive stream is fully read and the underlying DeflateStream acknowledges the ZLib suffix.
                if (_supportsZLib && _wasLastPayloadZLib && _receiveStream.Position != _receiveStream.Length)
                {
                    // We just need the inflater to read further so that it picks up the suffix and knows it's done.
                    _ = _receiveZLibStream!.Read(Array.Empty<byte>());
                }

                _receiveStream.Position = 0;
                _receiveStream.SetLength(0);
                do
                {
                    // See _limboCts for more info on cancellation.
                    var receiveTask = _ws.ReceiveAsync(_receiveBuffer, _limboCts.Token).AsTask();
                    using (var infiniteCts = Cts.Linked(cancellationToken))
                    {
                        var infiniteTask = Task.Delay(Timeout.Infinite, infiniteCts.Token);
                        await Task.WhenAny(infiniteTask, receiveTask).ConfigureAwait(false);
                        infiniteCts.Cancel();
                    }

                    if (cancellationToken.IsCancellationRequested)
                        throw new OperationCanceledException(cancellationToken);

                    var result = await receiveTask.ConfigureAwait(false);
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        var closeStatus = _ws.CloseStatus;
                        var closeMessage = _ws.CloseMessage;
                        try
                        {
                            await _ws.CloseOutputAsync(closeStatus.GetValueOrDefault(), closeMessage, default).ConfigureAwait(false);
                        }
                        catch { }

                        throw new WebSocketClosedException(closeStatus, closeMessage);
                    }

                    _receiveStream.Write(_receiveBuffer.AsSpan(0, result.Count));
                    if (!result.EndOfMessage)
                        continue;

                    if (result.MessageType != WebSocketMessageType.Binary)
                    {
                        _wasLastPayloadZLib = false;
                        _receiveStream.Position = 0;
                        return _receiveStream;
                    }

                    _receiveStream.TryGetBuffer(out var streamBuffer);

                    // We check the data for the ZLib flush which marks the end of the actual message.
                    if (streamBuffer.Count < 4 || BinaryPrimitives.ReadUInt32BigEndian(streamBuffer[^4..]) != 0x0000FFFF)
                        continue;

                    _wasLastPayloadZLib = true;
                    _receiveStream.Position = 0;
                    return _receiveZLibStream!;
                }
                while (!cancellationToken.IsCancellationRequested);

                throw new OperationCanceledException(cancellationToken);
            }
        }

        public async ValueTask CloseAsync(int closeStatus, string? closeMessage = null, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            using (await _sendSemaphore.EnterAsync(cancellationToken).ConfigureAwait(false))
            using (await _receiveSemaphore.EnterAsync(cancellationToken).ConfigureAwait(false))
            {
                if (_ws!.State != WebSocketState.Aborted)
                {
                    try
                    {
                        await _ws.CloseAsync(closeStatus, closeMessage, cancellationToken).ConfigureAwait(false);
                    }
                    catch { }
                }

                _limboCts?.Cancel();
                _limboCts?.Dispose();
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_isDisposed)
                return;

            using (await _sendSemaphore.EnterAsync().ConfigureAwait(false))
            using (await _receiveSemaphore.EnterAsync().ConfigureAwait(false))
            {
                if (_isDisposed)
                    return;

                _isDisposed = true;
                _receiveZLibStream?.Dispose();
                _receiveStream.Dispose();
                _ws?.Dispose();
            }
        }
    }
}
