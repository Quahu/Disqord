using System;
using System.Threading;

namespace Disqord
{
    internal sealed class EfficientCancellationTokenSource : IDisposable
    {
        public CancellationToken Token => _cts.Token;

        public bool IsCancellationRequested => _isCanceled || _cts.IsCancellationRequested;

        private readonly CancellationTokenSource _cts;
        private bool _isCanceled;
        private bool _isDisposed;

        public EfficientCancellationTokenSource()
        {
            _cts = new CancellationTokenSource();
        }

        private EfficientCancellationTokenSource(CancellationTokenSource linkedCts)
        {
            _cts = linkedCts;
        }

        public void Cancel()
        {
            if (_isCanceled || _isDisposed)
                return;

            _isCanceled = true;

            _cts.Cancel();
        }

        public void Dispose()
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            _cts.Dispose();
        }

        public static EfficientCancellationTokenSource CreateLinkedTokenSource(CancellationToken token1, CancellationToken token2)
        {
            var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(token1, token2);
            return new EfficientCancellationTokenSource(linkedCts);
        }
    }
}
