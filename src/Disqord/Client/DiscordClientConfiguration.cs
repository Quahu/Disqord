using Disqord.Logging;
using Disqord.Serialization.Json;
using Disqord.WebSocket;

namespace Disqord
{
    // TODO the default values would make more sense in the client's constructor probably
    public class DiscordClientConfiguration
    {
        /// <summary>
        ///     The <see cref="Disqord.MessageCache"/> the client should use to cache messages.
        ///     Defaults to a <see cref="DefaultMessageCache"/> with the capacity set to 100.
        /// </summary>
        public MessageCache MessageCache { get; set; } = new DefaultMessageCache(100);

        public int? ShardId { get; set; }

        public int? ShardCount { get; set; }

        public UserStatus Status { get; set; } = UserStatus.Online;

        public LocalActivity Activity { get; set; }

        public bool GuildSubscriptions { get; set; } = true;

        public ILogger Logger { get; set; }

        public IJsonSerializer Serializer { get; set; }

        public IWebSocketClient WebSocketClient { get; set; }

        public static DiscordClientConfiguration Default => new DiscordClientConfiguration();
    }
}
