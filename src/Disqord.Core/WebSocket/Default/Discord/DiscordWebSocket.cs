using System;
using System.Buffers.Binary;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Utilities.Threading;
using Microsoft.Extensions.Logging;

namespace Disqord.WebSocket.Default.Discord
{
    internal sealed partial class DiscordWebSocket : IDisposable
    {
        public const int ReceiveBufferSize = 8192;

        public ILogger Logger { get; }

        private readonly IWebSocketClientFactory _webSocketClientFactory;
        private readonly bool _supportsZLib;

        private IWebSocketClient _ws;

        /// <summary>
        ///     This is a fix for the ClientWebSocket being garbage and aborting itself on a cancelled ReceiveAsync (and possibly SendAsync)
        ///     rendering us unable to close the connection gracefully.
        ///     1. We create an infinite task that runs alongside the Send/ReceiveAsync() tasks and pass it the actual cancellation token just to signal cancellation.
        ///     2. We pass the Send/ReceiveAsync() task an essentially bogus cancellation token that gets cancelled when we close the connection,
        ///        allowing us to gracefully close and then have the websocket abort or whatever as we don't care about the state of it anymore.
        /// </summary>
        private Cts _limboCts;

        private readonly SemaphoreSlim _sendSemaphore;

        private readonly SemaphoreSlim _receiveSemaphore;
        private readonly byte[] _receiveBuffer;
        private readonly MemoryStream _receiveStream;
        private Stream _receiveZLibStream;

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

        public async ValueTask ConnectAsync(Uri url, CancellationToken token)
        {
            ThrowIfDisposed();

            _limboCts?.Cancel();
            _limboCts?.Dispose();
            _limboCts = new Cts();
            _ws?.Dispose();
            _ws = _webSocketClientFactory.CreateClient();
            if (_supportsZLib)
            {
                _receiveZLibStream?.Dispose();
                _receiveZLibStream =
#if NET5_0
                    CreateZLibStream(_receiveStream);
#else
                    new ZLibStream(_receiveStream, CompressionMode.Decompress);
#endif
            }

            await _ws.ConnectAsync(url, token).ConfigureAwait(false);
        }

        public async ValueTask SendAsync(Memory<byte> memory, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            await _sendSemaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                // See _limboCts for more info on cancellation.
                var sendTask = _ws.SendAsync(memory, WebSocketMessageType.Text, true, _limboCts.Token).AsTask();
                using (var infiniteCts = Cts.Linked(cancellationToken))
                {
                    var infiniteTask = Task.Delay(Timeout.Infinite, infiniteCts.Token);
                    var task = await Task.WhenAny(infiniteTask, sendTask).ConfigureAwait(false);
                    infiniteCts.Cancel();
                }

                cancellationToken.ThrowIfCancellationRequested();
            }
            finally
            {
                _sendSemaphore.Release();
            }
        }

        public async ValueTask<Stream> ReceiveAsync(CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            await _receiveSemaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                _receiveStream.Position = 0;
                _receiveStream.SetLength(0);
                do
                {
                    // See _limboCts for more info on cancellation.
                    var receiveTask = _ws.ReceiveAsync(_receiveBuffer, _limboCts.Token).AsTask();
                    using (var infiniteCts = Cts.Linked(cancellationToken))
                    {
                        var infiniteTask = Task.Delay(Timeout.Infinite, infiniteCts.Token);
                        var task = await Task.WhenAny(infiniteTask, receiveTask).ConfigureAwait(false);
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
                        _receiveStream.Position = 0;
                        return _receiveStream;
                    }

                    _receiveStream.TryGetBuffer(out var streamBuffer);

                    // We check the data for the ZLib flush which marks the end of the actual message.
                    if (streamBuffer.Count < 4 || BinaryPrimitives.ReadUInt32BigEndian(streamBuffer[^4..]) != 0x0000FFFF)
                        continue;

                    _receiveStream.Position = 0;
                    return _receiveZLibStream;
                }
                while (!cancellationToken.IsCancellationRequested);

                throw new OperationCanceledException(cancellationToken);
            }
            finally
            {
                _receiveSemaphore.Release();
            }
        }

        public async ValueTask CloseAsync(int closeStatus, string closeMessage = null, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            if (_ws.State != WebSocketState.Aborted)
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

        public void Dispose()
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
