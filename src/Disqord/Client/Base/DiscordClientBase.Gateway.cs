using System;
using System.Threading;
using System.Threading.Tasks;

namespace Disqord
{
    public abstract partial class DiscordClientBase : IRestDiscordClient, IAsyncDisposable
    {
        public abstract Task ConnectAsync();

        public abstract Task DisconnectAsync();

        public abstract Task SetPresenceAsync(UserStatus status);

        public abstract Task SetPresenceAsync(LocalActivity activity);

        public abstract Task SetPresenceAsync(UserStatus status, LocalActivity activity);
    }
}
