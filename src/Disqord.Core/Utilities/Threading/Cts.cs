using System;
using System.ComponentModel;
using System.Threading;

namespace Disqord.Utilities.Threading;

[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class Cts : IDisposable
{
    public CancellationToken Token => _cts.Token;

    public bool IsCancellationRequested => _isCanceled || _cts.IsCancellationRequested;

    private readonly CancellationTokenSource _cts;
    private bool _isCanceled;
    private bool _isDisposed;

    public Cts()
    {
        _cts = new CancellationTokenSource();
    }

    public Cts(TimeSpan delay)
    {
        _cts = new CancellationTokenSource(delay);
    }

    private Cts(CancellationTokenSource cts)
    {
        _cts = cts;
    }

    public void Cancel()
    {
        if (_isCanceled || _isDisposed)
            return;

        _isCanceled = true;
        _cts.Cancel();
    }

    public void CancelAfter(TimeSpan delay)
    {
        if (_isCanceled || _isDisposed)
            return;

        _cts.CancelAfter(delay);
    }

    public void Dispose()
    {
        if (_isDisposed)
            return;

        _isDisposed = true;
        _cts.Dispose();
    }

    public static Cts Linked(CancellationToken token)
    {
        var cts = CancellationTokenSource.CreateLinkedTokenSource(token);
        return new Cts(cts);
    }

    public static Cts Linked(CancellationToken token1, CancellationToken token2)
    {
        if (token1 == token2)
            return Linked(token1);

        var cts = CancellationTokenSource.CreateLinkedTokenSource(token1, token2);
        return new Cts(cts);
    }
}
