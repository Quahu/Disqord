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

        private readonly Timer _timeoutTimer;
        private readonly CancellationTokenRegistration _reg;

        public Waiter(Predicate<TEventArgs> predicate, TimeSpan timeout, CancellationToken cancellationToken)
        {
            _predicate = predicate;
            _tcs = new Tcs<TEventArgs>();

            static void CancellationCallback(object tuple)
            {
                var (tcs, token) = (ValueTuple<Tcs<TEventArgs>, CancellationToken>) tuple;
                tcs.Cancel(token);
            }

            _reg = cancellationToken.UnsafeRegister(CancellationCallback, (_tcs, cancellationToken));

            if (timeout != Timeout.InfiniteTimeSpan)
                _timeoutTimer = new Timer(CancellationCallback, (_tcs, new CancellationToken(true)), timeout, Timeout.InfiniteTimeSpan);
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
            _timeoutTimer?.Dispose();
            _reg.Dispose();
        }
    }
}
