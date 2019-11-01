using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Collections;
using Disqord.Logging;
using Disqord.Rest;
using Disqord.Serialization.Json;
using Qommon.Collections;

namespace Disqord
{
    public abstract partial class DiscordClientBase : IRestDiscordClient, IAsyncDisposable
    {
        public RestDownloadable<RestApplication> CurrentApplication => RestClient.CurrentApplication;

        /// <summary>
        ///     Gets the token type this client is using.
        /// </summary>
        public TokenType TokenType => RestClient.TokenType;

        public ILogger Logger { get; }

        public IJsonSerializer Serializer { get; }

        internal RestDiscordClient RestClient { get; }

        internal string Token => RestClient.ApiClient.Token;

        internal bool IsBot => TokenType == TokenType.Bot;

        RestDownloadable<RestCurrentUser> IRestDiscordClient.CurrentUser => RestClient.CurrentUser;

        internal DiscordClientBase(RestDiscordClient restClient,
            MessageCache messageCache = null,
            ILogger logger = null,
            IJsonSerializer serializer = null,
            IEnumerable<DiscordClientExtension> extensions = null)
        {
            if (restClient == null)
                throw new ArgumentNullException(nameof(restClient));

            State = new DiscordClientState(this, messageCache);
            Guilds = new ReadOnlyDictionary<Snowflake, CachedGuild>(State._guilds);
            Users = new ReadOnlyUpcastingDictionary<Snowflake, CachedSharedUser, CachedUser>(State._users);
            PrivateChannels = new ReadOnlyDictionary<Snowflake, CachedPrivateChannel>(State._privateChannels);
            DmChannels = new ReadOnlyOfTypeDictionary<Snowflake, CachedPrivateChannel, CachedDmChannel>(State._privateChannels);
            GroupChannels = new ReadOnlyOfTypeDictionary<Snowflake, CachedPrivateChannel, CachedGroupChannel>(State._privateChannels);

            RestClient = restClient;
            Logger = logger ?? restClient.Logger;
            Serializer = serializer ?? restClient.Serializer;
            Extensions = new Dictionary<Type, DiscordClientExtension>();
            if (extensions != null)
            {
                foreach (var extension in extensions)
                {
                    extension.Setup();

                    var type = extension.GetType();
                    if (!Extensions.TryAdd(type, extension))
                        throw new ArgumentException($"The extensions must not contain duplicate type instances ({type}).", nameof(extensions));
                }
            }
        }

        internal void Log(LogMessageSeverity severity, string message, Exception exception = null)
            => Logger.Log(this, new MessageLoggedEventArgs("Client", severity, message, exception));

        public void Dispose()
            => DisposeAsync().GetAwaiter().GetResult();

        public virtual async ValueTask DisposeAsync()
        {
            foreach (var extensionKvp in Extensions)
            {
                try
                {
                    await extensionKvp.Value.DisposeAsync().ConfigureAwait(false);
                }
                catch { }
            }

            RestClient.Dispose();
        }
    }
}
