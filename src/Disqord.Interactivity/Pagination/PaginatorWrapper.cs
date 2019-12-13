using System;
using System.Threading;
using System.Threading.Tasks;

namespace Disqord.Interactivity.Pagination
{
    internal sealed class PaginatorWrapper : IAsyncDisposable
    {
        public readonly PaginatorBase Paginator;
        private readonly InteractivityExtension _extension;
        private readonly CancellationTokenSource _cts;
        private readonly TimeSpan _timeout;
        private bool _isDisposed;

        public PaginatorWrapper(InteractivityExtension extension, PaginatorBase paginator, TimeSpan timeout)
        {
            Paginator = paginator;
            _extension = extension;
            _cts = new CancellationTokenSource(timeout);
            _timeout = timeout;
            _ = RunAsync();
        }

        private async Task RunAsync()
        {
            try
            {
                await Task.Delay(-1, _cts.Token);
            }
            catch { }

            if (_isDisposed)
                return;

            await _extension.ClosePaginatorAsync(Paginator.Message.Id);
        }

        public void Update()
            => _cts.CancelAfter(_timeout);

        public async ValueTask DisposeAsync()
        {
            if (_isDisposed)
                return;

            _isDisposed = true;
            try
            {
                _cts.Cancel();
            }
            catch { }
            _cts.Dispose();
            await Paginator.DisposeAsync().ConfigureAwait(false);
        }
    }
}
