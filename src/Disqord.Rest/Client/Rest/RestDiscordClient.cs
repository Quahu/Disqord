using System;
using Disqord.Logging;
using Disqord.Serialization.Json;

namespace Disqord.Rest
{
    public partial class RestDiscordClient : IDiscordClient
    {
        public RestDownloadable<RestCurrentUser> CurrentUser { get; }

        public RestDownloadable<RestApplication> CurrentApplication { get; }

        /// <summary>
        ///     Gets the token type this client is using.
        /// </summary>
        public TokenType TokenType => ApiClient.TokenType;

        public ILogger Logger => ApiClient.Logger;

        public IJsonSerializer Serializer => ApiClient.Serializer;

        internal readonly RestDiscordApiClient ApiClient;

        private string _ackToken;

        /// <summary>
        ///     Initialises a new <see cref="RestDiscordClient"/> without authorization.
        /// </summary>
        public static RestDiscordClient CreateWithoutAuthorization(ILogger logger = null, IJsonSerializer serializer = null)
            => new RestDiscordClient(logger, serializer);

        private RestDiscordClient(ILogger logger = null, IJsonSerializer serializer = null)
        {
            ApiClient = new RestDiscordApiClient(default, null, logger, serializer);
            CurrentUser = new RestDownloadable<RestCurrentUser>(
                _ => throw new InvalidOperationException("Cannot download the current user without providing an authorization token."));
            CurrentApplication = new RestDownloadable<RestApplication>(
                _ => throw new InvalidOperationException("Cannot download the current application without providing an authorization token."));
        }

        public RestDiscordClient(TokenType tokenType, string token, ILogger logger = null, IJsonSerializer serializer = null)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            ApiClient = new RestDiscordApiClient(tokenType, token, logger, serializer);
            CurrentUser = new RestDownloadable<RestCurrentUser>(async options =>
            {
                var model = await ApiClient.GetCurrentUserAsync(options).ConfigureAwait(false);
                return new RestCurrentUser(this, model);
            });

            CurrentApplication = tokenType == TokenType.Bot
                ? new RestDownloadable<RestApplication>(async options =>
                {
                    var model = await ApiClient.GetCurrentApplicationInformationAsync(options).ConfigureAwait(false);
                    return new RestApplication(this, model);
                })
                : new RestDownloadable<RestApplication>(
                    _ => throw new InvalidOperationException("Cannot download the current application without a bot authorization token."));
        }

        internal void Log(LogMessageSeverity severity, string message, Exception exception = null)
            => Logger.Log(this, new MessageLoggedEventArgs("Rest", severity, message, exception));

        public void Dispose()
        {
            ApiClient.Dispose();
        }
    }
}
