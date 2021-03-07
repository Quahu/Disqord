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
        public const int RECEIVE_BUFFER_SIZE = 8192;

        public ILogger Logger { get; }

        private readonly Func<IWebSocketClient> _webSocketClientFactory;
        private readonly bool _supportsZLib;

        private IWebSocketClient _ws;

        /// <summary>
        ///     This is a 200 IQ fix for the ClientWebSocket being garbage and aborting itself on a cancelled ReceiveAsync (and possibly SendAsync)
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
        private DeflateStream _receiveZLibStream;

        private bool _isDisposed;

        public DiscordWebSocket(
            ILogger logger, 
            Func<IWebSocketClient> webSocketClientFactory,
            bool supportsZLib = true)
        {
            Logger = logger;
            _webSocketClientFactory = webSocketClientFactory;
            _supportsZLib = supportsZLib;

            _sendSemaphore = new SemaphoreSlim(1, 1);

            _receiveSemaphore = new SemaphoreSlim(1, 1);
            _receiveBuffer = new byte[RECEIVE_BUFFER_SIZE];
            _receiveStream = new MemoryStream(RECEIVE_BUFFER_SIZE * 2);
        }

        private void ThrowIfDisposed()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(null, "The Discord web socket client has been disposed.");
        }

        public async Task ConnectAsync(Uri url, CancellationToken token)
        {
            ThrowIfDisposed();

            _limboCts?.Cancel();
            _limboCts?.Dispose();
            _limboCts = new Cts();
            _ws?.Dispose();
            _ws = _webSocketClientFactory();
            if (_supportsZLib)
            {
                _receiveZLibStream?.Dispose();
                _receiveZLibStream = _createZLibStream(_receiveStream);
            }
            await _ws.ConnectAsync(url, token).ConfigureAwait(false);
        }

        public async Task SendAsync(Memory<byte> memory, CancellationToken cancellationToken)
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

                if (cancellationToken.IsCancellationRequested)
                    throw new TaskCanceledException();
            }
            finally
            {
                _sendSemaphore.Release();
            }
        }

        public async Task<Stream> ReceiveAsync(CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            await _receiveSemaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                _receiveStream.Position = 0;
                _receiveStream.SetLength(0);
                WebSocketResult result;
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
                        throw new TaskCanceledException();

                    result = await receiveTask.ConfigureAwait(false);
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await _ws.CloseOutputAsync(_ws.CloseStatus.Value, _ws.CloseMessage, default).ConfigureAwait(false);
                        throw new WebSocketClosedException(_ws.CloseStatus, _ws.CloseMessage);
                    }

                    _receiveStream.Write(_receiveBuffer.AsSpan().Slice(0, result.Count));
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
                    {
                        Logger.LogInformation("Received a payload spanning multiple web socket messages.");
                        continue;
                    }

                    //// We rewind the stream to 0 or 2 based on whether the data has the ZLib header or not (DeflateStream breaks on the header).
                    //_receiveStream.Position = streamBuffer.Array[0] == 0x78 ? 2 : 0;
                    _receiveStream.Position = 0;
                    return _receiveZLibStream;
                }
                while (!cancellationToken.IsCancellationRequested);

                throw new TaskCanceledException();
            }
            finally
            {
                _receiveSemaphore.Release();
            }
        }

        public async Task CloseAsync(int closeStatus, string closeMessage = null, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            if (_ws.State != WebSocketState.Aborted)
            {
                try
                {
                    await _ws.CloseAsync(closeStatus, closeMessage, cancellationToken).ConfigureAwait(false);
                }
                catch
                { }
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