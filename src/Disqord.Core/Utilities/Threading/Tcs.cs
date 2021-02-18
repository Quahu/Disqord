using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Disqord.Utilities.Threading
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public sealed class Tcs
    {
        public Task Task => _tcs.Task;

        private readonly TaskCompletionSource<bool> _tcs;

        public Tcs()
        {
            _tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        }

        public void Complete()
            => _tcs.SetResult(true);

        public void Cancel()
            => _tcs.SetCanceled();

        public void Throw(Exception exception)
            => _tcs.SetException(exception);
    }
}
