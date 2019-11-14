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
    internal sealed class WebSocketClient : IDisposable
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

        private readonly ConcurrentQueue<WebSocketRequest> _messageQueue = new ConcurrentQueue<WebSocketRequest>();

        private bool _closed;
        private readonly object _closeLock = new object();

        private CancellationTokenSource _sendCts;
        private Task _sendTask;

        private CancellationTokenSource _receiveCts;

        private readonly MemoryStream _compressed;
        private readonly DeflateStream _deflate;
        private bool _isDisposed;

        public WebSocketClient()
        {
            _compressed = new MemoryStream();
            _deflate = new DeflateStream(_compressed, CompressionMode.Decompress, true);
        }

        public async Task ConnectAsync(Uri url, CancellationToken token)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(WebSocketClient));

            DisposeTokens();
            _closed = false;
            _ws?.Dispose();
            _ws = new ClientWebSocket();
            _ws.Options.KeepAliveInterval = TimeSpan.FromSeconds(10);
            await _ws.ConnectAsync(url, token).ConfigureAwait(false);
            _ = Task.Run(RunReceiveAsync);
        }

        public Task SendAsync(WebSocketRequest request)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(WebSocketClient));

            _messageQueue.Enqueue(request);
            if (_sendTask == null || _sendTask.IsCompleted)
            {
                _sendCts?.Dispose();
                _sendCts = new CancellationTokenSource();
                _sendTask = Task.Run(RunSendAsync);
            }

            return request.WaitAsync();
        }

        private async Task RunSendAsync()
        {
            while (_messageQueue.TryDequeue(out var request) && !_sendCts.IsCancellationRequested)
            {
                using (var linkedSource = CancellationTokenSource.CreateLinkedTokenSource(_sendCts.Token, request.CancellationToken))
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
            _compressed.Position = 0;
            _compressed.SetLength(0);

            _receiveCts = new CancellationTokenSource();
            var buffer = ArrayPool<byte>.Shared.Rent(RECEIVE_BUFFER_SIZE);
            var bufferMemory = buffer.AsMemory(0, RECEIVE_BUFFER_SIZE);
            try
            {
                while (!_receiveCts.IsCancellationRequested && _ws.State == WebSocketState.Open)
                {
                    ValueWebSocketReceiveResult result;
                    do
                    {
                        try
                        {
                            result = await _ws.ReceiveAsync(bufferMemory, _receiveCts.Token).ConfigureAwait(false);
                        }
                        catch (Exception ex)
                        {
                            DisposeTokens();
                            if (_ws.State == WebSocketState.Open)
                            {
                                try
                                {
                                    await CloseAsync().ConfigureAwait(false);
                                }
                                catch { }
                            }
                            try
                            {
                                await _closedEvent.InvokeAsync(new WebSocketClosedEventArgs(null, null, ex)).ConfigureAwait(false);
                            }
                            catch { }
                            return;
                        }

                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            DisposeTokens();
                            try
                            {
                                await CloseAsync().ConfigureAwait(false);
                            }
                            catch { }
                            try
                            {
                                await _closedEvent.InvokeAsync(new WebSocketClosedEventArgs(_ws.CloseStatus, _ws.CloseStatusDescription, null)).ConfigureAwait(false);
                            }
                            catch { }
                            return;
                        }
                        else
                        {
                            await _compressed.WriteAsync(bufferMemory.Slice(0, result.Count), _receiveCts.Token).ConfigureAwait(false);
                        }
                    }
                    while (!result.EndOfMessage && !_receiveCts.IsCancellationRequested);

                    var isZlib = false;
                    var hasZlibHeader = false;
                    if (result.MessageType == WebSocketMessageType.Binary)
                    {
                        _compressed.TryGetBuffer(out var streamBuffer);
                        if (streamBuffer.Array[0] == ZLIB_HEADER)
                        {
                            hasZlibHeader = true;
                            isZlib = true;
                        }

                        if (streamBuffer.Count > 4
                            && streamBuffer[^1] == ZLIB_SUFFIX
                            && streamBuffer[^2] == ZLIB_SUFFIX)
                        {
                            isZlib = true;
                        }
                    }

                    _compressed.Position = hasZlibHeader ? 2 : 0;

                    try
                    {
                        await _messageReceivedEvent.InvokeAsync(new WebSocketMessageReceivedEventArgs(isZlib ? (Stream) _deflate : _compressed)).ConfigureAwait(false);
                    }
                    catch { }

                    _compressed.Position = 0;
                    _compressed.SetLength(0);
                }
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }

        public async Task CloseAsync()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(WebSocketClient));

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
                    await _ws.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    //$"Exception while closing the websocket:";
                }
                await _closedEvent.InvokeAsync(new WebSocketClosedEventArgs(WebSocketCloseStatus.NormalClosure, string.Empty, null)).ConfigureAwait(false);
            }

            DisposeTokens();
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

        public void Dispose()
        {
            if (_isDisposed)
                return;

            _isDisposed = true;
            DisposeTokens();
            _compressed.Dispose();
            _deflate.Dispose();
            _ws?.Dispose();
        }
    }
}
