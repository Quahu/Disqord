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
            _semaphore = _semaphores.GetOrAdd(gateway._client.Token, _ => new SemaphoreSlim(1, 1));
        }

        public async Task WaitAsync()
        {
            Task task;
            lock (_semaphore)
            {
                task = _semaphore.WaitAsync();
                if (_semaphore.CurrentCount == 0)
                {
                    _gateway.Log(LogMessageSeverity.Information, "Delaying identifying...");
                }
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
