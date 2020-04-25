using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.IO;
using System.IO.Compression;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Qommon.Events;

namespace Disqord.WebSocket
{
    internal sealed class WebSocketClient : IAsyncDisposable
    {
        public event AsynchronousEventHandler<WebSocketMessageReceivedEventArgs> MessageReceived
        {
            add => _messageReceivedEvent.Hook(value);
            remove => _messageReceivedEvent.Unhook(value);
        }
        private readonly AsynchronousEvent<WebSocketMessageReceivedEventArgs> _messageReceivedEvent = new AsynchronousEvent<WebSocketMessageReceivedEventArgs>();

        public event AsynchronousEventHandler<WebSocketClosedEventArgs> Closed
        {
            add => _closedEvent.Hook(value);
            remove => _closedEvent.Unhook(value);
        }
        private readonly AsynchronousEvent<WebSocketClosedEventArgs> _closedEvent = new AsynchronousEvent<WebSocketClosedEventArgs>();

        public const byte ZLIB_HEADER = 0x78;

        public const byte ZLIB_SUFFIX = 0xFF;

        public const int RECEIVE_BUFFER_SIZE = 8192;

        private ClientWebSocket _ws;

        private readonly ConcurrentQueue<WebSocketRequest> _requestQueue = new ConcurrentQueue<WebSocketRequest>();

        private bool _closed;
        private readonly object _closeLock = new object();
        private readonly object _sendLock = new object();

        private CancellationTokenSource _sendCts;
        private Task _sendTask;

        private CancellationTokenSource _receiveCts;
        private Task _receiveTask;

        private readonly MemoryStream _compressedStream;
        private readonly DeflateStream _deflateStream;
        private bool _isDisposed;

        public WebSocketClient()
        {
            _compressedStream = new MemoryStream();
            _deflateStream = new DeflateStream(_compressedStream, CompressionMode.Decompress, true);
        }

        private void ThrowIfDisposed()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(null, "The web socket client has been disposed.");
        }

        public async Task ConnectAsync(Uri url, CancellationToken token)
        {
            ThrowIfDisposed();

            DisposeTokens();
            _closed = false;
            _ws?.Abort();
            _ws?.Dispose();
            _ws = new ClientWebSocket();
            _ws.Options.KeepAliveInterval = TimeSpan.FromSeconds(10);
            await _ws.ConnectAsync(url, token).ConfigureAwait(false);
            _receiveTask = Task.Run(RunReceiveAsync);
        }

        public Task SendAsync(WebSocketRequest request)
        {
            ThrowIfDisposed();

            _requestQueue.Enqueue(request);
            lock (_sendLock)
            {
                if (_sendTask == null || _sendTask.IsCompleted)
                {
                    _sendCts?.Dispose();
                    _sendCts = new CancellationTokenSource();
                    _sendTask = Task.Run(RunSendAsync);
                }
            }

            return request.WaitAsync();
        }

        private async Task RunSendAsync()
        {
            CancellationTokenSource cts;
            lock (_sendLock)
            {
                cts = _sendCts;
                if (cts == null)
                    return;
            }

            while (!cts.IsCancellationRequested && _requestQueue.TryDequeue(out var request))
            {
                using (var linkedSource = CancellationTokenSource.CreateLinkedTokenSource(cts.Token, request.CancellationToken))
                {
                    try
                    {
                        await _ws.SendAsync(request.Message, WebSocketMessageType.Text, true, linkedSource.Token).ConfigureAwait(false);
                        request.SetComplete();
                    }
                    catch (Exception ex)
                    {
                        request.SetException(ex);
                    }
                }
            }
        }

        private async Task RunReceiveAsync()
        {
            _compressedStream.Position = 0;
            _compressedStream.SetLength(0);

            _receiveCts = new CancellationTokenSource();
            var receiveToken = _receiveCts.Token;

            var buffer = ArrayPool<byte>.Shared.Rent(RECEIVE_BUFFER_SIZE);
            var bufferMemory = buffer.AsMemory(0, RECEIVE_BUFFER_SIZE);
            try
            {
                while (!receiveToken.IsCancellationRequested && _ws.State == WebSocketState.Open)
                {
                    ValueWebSocketReceiveResult result;
                    do
                    {
                        try
                        {
                            result = await _ws.ReceiveAsync(bufferMemory, receiveToken).ConfigureAwait(false);
                        }
                        catch (Exception ex)
                        {
                            await _closedEvent.InvokeAsync(new WebSocketClosedEventArgs(_ws.CloseStatus, _ws.CloseStatusDescription, ex)).ConfigureAwait(false);
                            return;
                        }

                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            try
                            {
                                await CloseAsync().ConfigureAwait(false);
                            }
                            catch { }
                            return;
                        }
                        else
                        {
                            await _compressedStream.WriteAsync(bufferMemory.Slice(0, result.Count), receiveToken).ConfigureAwait(false);
                        }
                    }
                    while (!result.EndOfMessage && !receiveToken.IsCancellationRequested);

                    var isZlib = false;
                    var hasZlibHeader = false;
                    if (result.MessageType == WebSocketMessageType.Binary)
                    {
                        _compressedStream.TryGetBuffer(out var streamBuffer);
                        if (streamBuffer.Array[0] == ZLIB_HEADER)
                        {
                            hasZlibHeader = true;
                            isZlib = true;
                        }

                        if (streamBuffer.Count > 4
                            && streamBuffer[^1] == ZLIB_SUFFIX
                            && streamBuffer[^2] == ZLIB_SUFFIX
                            && streamBuffer[^3] == 0
                            && streamBuffer[^4] == 0)
                        {
                            isZlib = true;
                        }
                    }

                    _compressedStream.Position = hasZlibHeader ? 2 : 0;

                    try
                    {
                        var stream = isZlib ? (Stream) _deflateStream : _compressedStream;
                        await _messageReceivedEvent.InvokeAsync(new WebSocketMessageReceivedEventArgs(stream)).ConfigureAwait(false);
                    }
                    catch { }

                    _compressedStream.Position = 0;
                    _compressedStream.SetLength(0);
                }
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }

        public async Task CloseAsync()
        {
            ThrowIfDisposed();
            var closeStatus = _ws.CloseStatus;
            var closeDescription = _ws.CloseStatusDescription;
            DisposeTokens();

            lock (_closeLock)
            {
                if (_closed)
                    return;

                _closed = true;
            }

            if (_ws != null && _ws.State != WebSocketState.Aborted)
            {
                try
                {
                    // https://github.com/discord/discord-api-docs/issues/1472
                    await _ws.CloseAsync((WebSocketCloseStatus) 4000, string.Empty, CancellationToken.None).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    //$"Exception while closing the websocket:";
                }

                if (closeStatus == null)
                {
                    closeStatus = (WebSocketCloseStatus) 4000;
                    closeDescription = "Manual close after a requested reconnect.";
                }

                await _closedEvent.InvokeAsync(new WebSocketClosedEventArgs(closeStatus, closeDescription, null)).ConfigureAwait(false);
            }
        }

        public void DisposeTokens()
        {
            try
            {
                _sendCts?.Cancel();
            }
            catch { }
            _sendCts?.Dispose();
            _sendCts = null;
            try
            {
                _receiveCts?.Cancel();
            }
            catch { }
            _receiveCts?.Dispose();
            _receiveCts = null;
        }

        public async ValueTask DisposeAsync()
        {
            if (_isDisposed)
                return;

            _isDisposed = true;
            DisposeTokens();
            _compressedStream.Dispose();
            _deflateStream.Dispose();
            _ws?.Dispose();
        }
    }
}
