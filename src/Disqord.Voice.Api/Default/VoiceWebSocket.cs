using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Utilities.Threading;
using Disqord.WebSocket;
using Microsoft.Extensions.Logging;
using Qommon;
using Qommon.Threading;

namespace Disqord.Voice.Api.Default;

internal sealed class VoiceWebSocket(
    ILogger logger,
    IWebSocketClientFactory webSocketClientFactory) : IAsyncDisposable
{
    public const int ReceiveBufferSize = 8192;

    public ILogger Logger { get; } = logger;

    private IWebSocketClient? _ws;

    /// <summary>
    ///     This is a fix for the ClientWebSocket being garbage and aborting itself on a cancelled ReceiveAsync (and possibly SendAsync)
    ///     rendering us unable to close the connection gracefully.
    ///     1. We create an infinite task that runs alongside the Send/ReceiveAsync() tasks and pass it the actual cancellation token just to signal cancellation.
    ///     2. We pass the Send/ReceiveAsync() task an essentially bogus cancellation token that gets cancelled when we close the connection,
    ///        allowing us to gracefully close and then have the websocket abort or whatever as we don't care about the state of it anymore.
    /// </summary>
    private Cts? _limboCts;

    private readonly SemaphoreSlim _sendSemaphore = new(1, 1);

    private readonly SemaphoreSlim _receiveSemaphore = new(1, 1);
    private readonly byte[] _receiveBuffer = new byte[ReceiveBufferSize];
    private readonly MemoryStream _receiveStream = new(ReceiveBufferSize * 2);

    private bool _isDisposed;

    private void ThrowIfDisposed()
    {
        if (_isDisposed)
            throw new ObjectDisposedException(null, "The voice web socket client has been disposed.");
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
            _ws = webSocketClientFactory.CreateClient();

            await _ws.ConnectAsync(url, cancellationToken).ConfigureAwait(false);
        }
    }

    public async ValueTask SendAsync(ReadOnlyMemory<byte> memory, WebSocketMessageType messageType, CancellationToken cancellationToken)
    {
        using (await _sendSemaphore.EnterAsync(cancellationToken).ConfigureAwait(false))
        {
            ThrowIfDisposed();

            // See _limboCts for more info on cancellation.
            var sendTask = _ws!.SendAsync(memory, messageType, true, _limboCts!.Token).AsTask();
            using (var infiniteCts = Cts.Linked(cancellationToken))
            {
                var infiniteTask = Task.Delay(Timeout.Infinite, infiniteCts.Token);
                await Task.WhenAny(infiniteTask, sendTask).ConfigureAwait(false);
                infiniteCts.Cancel();
            }

            cancellationToken.ThrowIfCancellationRequested();
        }
    }

    public async ValueTask<(MemoryStream Stream, bool IsBinary)> ReceiveAsync(CancellationToken cancellationToken)
    {
        using (await _receiveSemaphore.EnterAsync(cancellationToken).ConfigureAwait(false))
        {
            ThrowIfDisposed();

            Guard.IsNotNull(_ws);
            Guard.IsNotNull(_limboCts);

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

                _receiveStream.Position = 0;
                return (_receiveStream, result.MessageType == WebSocketMessageType.Binary);
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
            _receiveStream.Dispose();
            _ws?.Dispose();
        }
    }
}
