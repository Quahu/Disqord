using System;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord
{
    public abstract partial class DiscordClientBase : IRestDiscordClient, IAsyncDisposable
    {
        public virtual TimeSpan? Latency => _client?.Latency;

        public virtual Task RunAsync(CancellationToken cancellationToken = default)
            => _client?.RunAsync(cancellationToken) ?? throw new PlatformNotSupportedException();

        public virtual Task StopAsync()
            => _client?.StopAsync() ?? throw new PlatformNotSupportedException();

        public virtual Task SetPresenceAsync(UserStatus status)
            => _client?.SetPresenceAsync(status) ?? throw new PlatformNotSupportedException();

        public virtual Task SetPresenceAsync(LocalActivity activity)
            => _client?.SetPresenceAsync(activity) ?? throw new PlatformNotSupportedException();

        public virtual Task SetPresenceAsync(UserStatus status, LocalActivity activity)
            => _client?.SetPresenceAsync(status, activity) ?? throw new PlatformNotSupportedException();

        internal virtual Task<string> GetGatewayAsync(bool isNewSession)
            => _client.GetGatewayAsync(isNewSession) ?? throw new PlatformNotSupportedException();
    }
}
