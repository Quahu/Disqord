using System.Threading;
using System.Threading.Tasks;
using Disqord.Collections;
using Disqord.Logging;

namespace Disqord.Client.Gateway
{
    internal sealed class IdentifyLock
    {
        private static readonly LockedDictionary<string, SemaphoreSlim> _semaphores = new LockedDictionary<string, SemaphoreSlim>(1);

        private readonly DiscordClientGateway _gateway;

        private readonly SemaphoreSlim _semaphore;

        public IdentifyLock(DiscordClientGateway gateway)
        {
            _gateway = gateway;

            // TODO: tie this to application id?
            // TODO: persistence?
            _semaphore = _semaphores.GetOrAdd(gateway.Client.Token, _ => new SemaphoreSlim(1, 1));
        }

        public async Task WaitAsync()
        {
            Task task;
            lock (_semaphore)
            {
                if (_semaphore.CurrentCount == 0)
                    _gateway.Log(LogMessageSeverity.Information, "Delaying identifying...");

                task = _semaphore.WaitAsync();
            }

            await task.ConfigureAwait(false);
            _ = ReleaseAsync();
        }

        private async Task ReleaseAsync()
        {
            await Task.Delay(5500).ConfigureAwait(false);
            lock (_semaphore)
            {
                _semaphore.Release();
            }
        }
    }
}
