using System;
using System.Threading;
using System.Threading.Tasks;

namespace Disqord.WebSocket
{
    internal sealed class WebSocketRequest
    {
        public ReadOnlyMemory<byte> Message { get; }

        public CancellationToken Token { get; }

        private readonly TaskCompletionSource<bool> _tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

        public WebSocketRequest(ReadOnlyMemory<byte> message, CancellationToken cancellationToken)
        {
            Message = message;
            Token = cancellationToken;
        }

        public async Task WaitAsync()
            => await _tcs.Task.ConfigureAwait(false);

        public void SetComplete()
            => _tcs.SetResult(true);

        public void SetException(Exception exception)
            => _tcs.SetException(exception);
    }
}
