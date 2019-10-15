using Disqord.Serialization.Json;

namespace Disqord
{
    public class DiscordClientConfiguration
    {
        public int MessageCacheSize { get; set; } = 100;

        public int ShardId { get; set; }

        public int ShardAmount { get; set; }

        public UserStatus Status { get; set; } = UserStatus.Online;

        public LocalActivity Activity { get; set; }

        public bool GuildSubscriptions { get; set; } = true;

        public IJsonSerializer Serializer { get; set; }

        public static DiscordClientConfiguration Default => new DiscordClientConfiguration();
    }
}
