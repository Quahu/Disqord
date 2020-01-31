namespace Disqord
{
    public class DiscordClientConfiguration : DiscordClientBaseConfiguration
    {
        public Optional<int> ShardId { get; set; }

        public Optional<int> ShardCount { get; set; }

        public static DiscordClientConfiguration Default => new DiscordClientConfiguration();
    }
}
