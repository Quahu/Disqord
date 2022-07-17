using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace Disqord.Utilities.Threading;

[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class Tcs<TResult>
{
    public Task<TResult> Task => _tcs.Task;

    private readonly TaskCompletionSource<TResult> _tcs;

    public Tcs()
    {
        _tcs = new TaskCompletionSource<TResult>(TaskCreationOptions.RunContinuationsAsynchronously);
    }

    public bool Complete(TResult result)
        => _tcs.TrySetResult(result);

    public bool Cancel(CancellationToken cancellationToken = default)
        => _tcs.TrySetCanceled(cancellationToken);

    public bool Throw(Exception exception)
        => _tcs.TrySetException(exception);
}