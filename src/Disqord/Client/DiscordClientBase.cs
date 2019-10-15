using System;
using Disqord.Logging;
using Disqord.Rest;

namespace Disqord
{
    public abstract partial class DiscordClientBase : IDiscordClient
    {
        public abstract RestDiscordClient RestClient { get; protected set; }

        /// <summary>
        ///     Gets the currently logged user.
        /// </summary>
        public CachedCurrentUser CurrentUser { get; protected set; }

        public RestDownloadable<RestApplication> CurrentApplication => RestClient.CurrentApplication;

        public ILogger Logger => RestClient.Logger;

        RestDownloadable<RestCurrentUser> IDiscordClient.CurrentUser => RestClient.CurrentUser;

        /// <summary>
        ///     Gets the <see cref="Disqord.TokenType"/> this client is using.
        /// </summary>
        public TokenType TokenType => RestClient.TokenType;

        internal virtual void Log(LogMessageSeverity severity, string message, Exception exception = null)
            => Logger.Log(this, new MessageLoggedEventArgs("Gateway", severity, message, exception));

        public abstract void Dispose();
    }
}
