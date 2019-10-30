using System;
using System.Threading.Tasks;
using Disqord.Logging;
using Disqord.Rest;

namespace Disqord
{
    public abstract partial class DiscordClientBase : IRestDiscordClient, IAsyncDisposable
    {
        internal abstract RestDiscordClient RestClient { get; }

        /// <summary>
        ///     Gets the currently logged-in user.
        /// </summary>
        public CachedCurrentUser CurrentUser { get; protected set; }

        public RestDownloadable<RestApplication> CurrentApplication => RestClient.CurrentApplication;

        public ILogger Logger => RestClient.Logger;

        RestDownloadable<RestCurrentUser> IRestDiscordClient.CurrentUser => RestClient.CurrentUser;

        /// <summary>
        ///     Gets the <see cref="Disqord.TokenType"/> this client is using.
        /// </summary>
        public TokenType TokenType => RestClient.TokenType;

        internal void Log(LogMessageSeverity severity, string message, Exception exception = null)
            => Logger.Log(this, new MessageLoggedEventArgs("Gateway", severity, message, exception));

        void IDisposable.Dispose()
            => DisposeAsync().GetAwaiter().GetResult();

        public abstract ValueTask DisposeAsync();
    }
}
