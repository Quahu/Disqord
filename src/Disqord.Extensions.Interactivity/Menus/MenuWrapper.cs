using System;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Extensions.Interactivity.Menus;

namespace Disqord.Extensions.Interactivity.Pagination
{
    internal sealed class MenuWrapper : IAsyncDisposable
    {
        public readonly MenuBase Menu;
        private readonly InteractivityExtension _extension;
        private readonly CancellationTokenSource _cts;
        private readonly TimeSpan _timeout;
        private bool _isDisposed;

        public MenuWrapper(InteractivityExtension extension, MenuBase menu, TimeSpan timeout)
        {
            Menu = menu;
            _extension = extension;
            _cts = new CancellationTokenSource(timeout);
            _timeout = timeout;
            _ = RunAsync();
        }

        private async Task RunAsync()
        {
            try
            {
                await Task.Delay(-1, _cts.Token).ConfigureAwait(false);
            }
            catch { }

            if (_isDisposed)
                return;

            await _extension.StopMenuAsync(Menu.Message.Id).ConfigureAwait(false);
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
            await Menu.DisposeAsync().ConfigureAwait(false);
        }
    }
}
