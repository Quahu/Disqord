using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace Disqord.Utilities.Threading;

[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class Tcs
{
    public Task Task => _tcs.Task;

    private readonly TaskCompletionSource _tcs;

    public Tcs()
    {
        _tcs = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
    }

    public bool Complete()
        => _tcs.TrySetResult();

    public bool Cancel(CancellationToken cancellationToken = default)
        => _tcs.TrySetCanceled(cancellationToken);

    public bool Throw(Exception exception)
        => _tcs.TrySetException(exception);
}