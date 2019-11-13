using System;
using System.Threading;
using System.Threading.Tasks;

namespace Disqord.WebSocket
{
    internal sealed class WebSocketRequest
    {
        public ReadOnlyMemory<byte> Message { get; }

        public CancellationToken CancellationToken { get; }

        private readonly TaskCompletionSource<bool> _tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

        internal WebSocketRequest(ReadOnlyMemory<byte> message, CancellationToken cancellationToken)
        {
            Message = message;
            CancellationToken = cancellationToken;
        }

        internal Task WaitAsync()
            => _tcs.Task;

        public void SetComplete()
            => _tcs.SetResult(true);

        public void SetException(Exception exception)
            => _tcs.SetException(exception);
    }
}
