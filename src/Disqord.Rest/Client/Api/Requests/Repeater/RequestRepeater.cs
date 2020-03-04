using System;
using System.Threading;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    internal abstract class RequestRepeater : IDisposable
    {
        protected readonly CancellationTokenSource _cts;
        private readonly Func<CancellationToken, Task> _func;
        private readonly TimeSpan _delay;
        private bool _disposed;

        protected RequestRepeater(Func<CancellationToken, Task> func, TimeSpan delay, TimeSpan cancelAfter)
        {
            _func = func;
            _delay = delay;
            _cts = new CancellationTokenSource(cancelAfter);
            _ = Task.Run(SendAsync);
        }

        protected RequestRepeater(Func<CancellationToken, Task> func, TimeSpan delay)
        {
            _func = func;
            _delay = delay;
            _cts = new CancellationTokenSource();
            _ = Task.Run(SendAsync);
        }

        ~RequestRepeater()
        {
            Dispose();
        }

        private async Task SendAsync()
        {
            while (!_cts.IsCancellationRequested)
            {
                var task = _func(_cts.Token);
                if (task != null)
                {
                    try
                    {
                        await task.ConfigureAwait(false);
                    }
                    catch
                    {
                        Dispose();
                        return;
                    }
                }

                await Task.Delay(_delay, _cts.Token).ConfigureAwait(false);
            }
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;
            GC.SuppressFinalize(true);
            _cts.Cancel();
            _cts.Dispose();
        }
    }
}
