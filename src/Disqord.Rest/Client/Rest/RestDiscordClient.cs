using System;
using Disqord.Logging;
using Disqord.Serialization.Json;

namespace Disqord.Rest
{
    public partial class RestDiscordClient : IRestDiscordClient
    {
        public RestFetchable<RestCurrentUser> CurrentUser { get; }

        public RestFetchable<RestApplication> CurrentApplication { get; }

        /// <summary>
        ///     Gets the token type this client is using.
        ///     Will throw an exception, if <see cref="HasAuthorization"/> returns <see langword="false"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     This client has no authorization.
        /// </exception>
        public TokenType TokenType
        {
            get
            {
                if (!HasAuthorization)
                    throw new InvalidOperationException("This client has no authorization.");

                return ApiClient._tokenType.Value;
            }
        }

        /// <summary>
        ///     Gets whether this client has an authorization token.
        ///     Returns <see langword="true"/> for all normal clients and for logged in
        ///     clients created using <see cref="CreateWithoutAuthorization(RestDiscordClientConfiguration)"/>.
        /// </summary>
        public bool HasAuthorization => ApiClient._tokenType != null;

        public ILogger Logger => ApiClient.Logger;

        public IJsonSerializer Serializer => ApiClient.Serializer;

        internal readonly RestDiscordApiClient ApiClient;

        private string _ackToken;

        /// <summary>
        ///     Initialises a new <see cref="RestDiscordClient"/> without authorization.
        /// </summary>
        public static RestDiscordClient CreateWithoutAuthorization(RestDiscordClientConfiguration configuration = null)
            => new RestDiscordClient(null, null, configuration);

        private RestDiscordClient(TokenType? optionalTokenType, string token, RestDiscordClientConfiguration configuration = null)
        {
            ApiClient = new RestDiscordApiClient(optionalTokenType, token, configuration ?? new RestDiscordClientConfiguration());
            CurrentUser = RestFetchable.Create(this, async (@this, options) =>
            {
                var model = await @this.ApiClient.GetCurrentUserAsync(options).ConfigureAwait(false);
                return new RestCurrentUser(@this, model);
            });
            CurrentApplication = RestFetchable.Create(this, async (@this, options) =>
            {
                //if (TokenType != TokenType.Bot)
                //    throw new InvalidOperationException("Cannot download the current application without a bot authorization token.");

                var model = await @this.ApiClient.GetCurrentApplicationInformationAsync(options).ConfigureAwait(false);
                return new RestApplication(@this, model);
            });
        }

        public RestDiscordClient(TokenType tokenType, string token, RestDiscordClientConfiguration configuration = null)
            : this(optionalTokenType: tokenType, token ?? throw new ArgumentNullException(nameof(token)), configuration)
        { }

        internal void Log(LogMessageSeverity severity, string message, Exception exception = null)
            => Logger.Log(this, new MessageLoggedEventArgs("Rest", severity, message, exception));

        public void Dispose()
        {
            ApiClient.Dispose();
        }
    }
}
