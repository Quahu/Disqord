using System;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Utilities.Threading;

namespace Disqord.Extensions.Interactivity
{
    internal class Waiter<TEventArgs> : IDisposable
        where TEventArgs : EventArgs
    {
        public Task<TEventArgs> Task => _tcs.Task;

        private readonly Predicate<TEventArgs> _predicate;
        private readonly Tcs<TEventArgs> _tcs;

        private readonly Cts _timeoutCts;
        private readonly CancellationTokenRegistration _timeoutRegistration;
        private readonly CancellationTokenRegistration _registration;

        public Waiter(Predicate<TEventArgs> predicate, TimeSpan timeout, CancellationToken cancellationToken)
        {
            _predicate = predicate;
            _tcs = new Tcs<TEventArgs>();

            static void CancelationCallback(object tuple)
            {
                var (tcs, token) = (ValueTuple<Tcs<TEventArgs>, CancellationToken>) tuple;
                tcs.Cancel(token);
            }

            if (timeout != Timeout.InfiniteTimeSpan)
            {
                _timeoutCts = new Cts(timeout);
                _timeoutRegistration = _timeoutCts.Token.Register(CancelationCallback, (_tcs, _timeoutCts.Token));

            }

            _registration = cancellationToken.Register(CancelationCallback, (_tcs, cancellationToken));
        }

        public bool TryComplete(TEventArgs e)
        {
            try
            {
                if (_predicate != null)
                {
                    if (!_predicate(e))
                        return false;
                }
            }
            catch (Exception ex)
            {
                _tcs.Throw(ex);
                return true;
            }

            return _tcs.Complete(e);
        }

        public void Dispose()
        {
            if (_timeoutCts != null)
            {
                _timeoutCts.Dispose();
                _timeoutRegistration.Dispose();
            }

            _registration.Dispose();
        }
    }
}
