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

        private readonly object _sendLock = new object();

        private EfficientCancellationTokenSource _sendCts;
        private Task _sendTask;

        private EfficientCancellationTokenSource _receiveCts;
        private Task _receiveTask;

        private readonly MemoryStream _compressedStream;
        private readonly DeflateStream _deflateStream;
        private bool _isDisposed;
        private WebSocketCloseStatus? _closeStatus;
        private string _closeDescription;

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
            _closeStatus = null;
            _closeDescription = null;

            if (Library.Debug.TimedWebSocketConnect)
            {
                using (var cts = new CancellationTokenSource())
                using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cts.Token, token))
                {
                    // I don't trust any of these at this point
                    _ws?.Dispose();
                    _ws = new ClientWebSocket();
                    _ws.Options.KeepAliveInterval = TimeSpan.FromSeconds(10);
                    var connectTask = Task.Run(() => _ws.ConnectAsync(url, linkedCts.Token), linkedCts.Token);
                    var delayTask = Task.Delay(10_000, linkedCts.Token);
                    var task = await Task.WhenAny(connectTask, delayTask).ConfigureAwait(false);
                    linkedCts.Cancel();

                    if (task == delayTask)
                        throw new TaskCanceledException("Timed out waiting for ClientWebSocket's ConnectAsync.");
                }
            }
            else
            {
                _ws?.Dispose();
                _ws = new ClientWebSocket();
                _ws.Options.KeepAliveInterval = TimeSpan.FromSeconds(10);
                await _ws.ConnectAsync(url, token).ConfigureAwait(false);
            }

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
                    _sendCts = new EfficientCancellationTokenSource();
                    _sendTask = Task.Run(RunSendAsync);
                }
            }

            return request.WaitAsync();
        }

        private async Task RunSendAsync()
        {
            var token = _sendCts.Token;
            while (!token.IsCancellationRequested && _requestQueue.TryDequeue(out var request))
            {
                using (var linkedSource = CancellationTokenSource.CreateLinkedTokenSource(token, request.CancellationToken))
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

            _receiveCts = new EfficientCancellationTokenSource();
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
                            if (_closeStatus != (WebSocketCloseStatus) 4000)
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

            if (_closeStatus == (WebSocketCloseStatus) 4000)
                return;

            _closeStatus = _ws.CloseStatus;
            _closeDescription = _ws.CloseStatusDescription;
            DisposeTokens();

            if (_closeStatus == null)
            {
                _closeStatus = (WebSocketCloseStatus) 4000;
                _closeDescription = "Manual close after a requested reconnect.";
            }

            if (_ws.State != WebSocketState.Aborted)
            {
                try
                {
                    // https://github.com/discord/discord-api-docs/issues/1472
                    await _ws.CloseAsync(_closeStatus.Value, _closeDescription, CancellationToken.None).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
#if DEBUG
                    Console.WriteLine($"Exception while closing the websocket:\n{ex}");
#endif
                }
            }

            await _closedEvent.InvokeAsync(new WebSocketClosedEventArgs(_closeStatus, _closeDescription, null)).ConfigureAwait(false);
        }

        public void DisposeTokens()
        {
            _sendCts?.Cancel();
            _sendCts?.Dispose();
            _sendCts = null;
            _receiveCts?.Cancel();
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
