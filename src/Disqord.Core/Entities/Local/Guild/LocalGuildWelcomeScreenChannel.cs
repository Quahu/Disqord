namespace Disqord
{
    public class LocalGuildWelcomeScreenChannel
    {
        public Snowflake ChannelId { get; }

        public string Description { get; }

        public Snowflake? EmojiId { get; }

        public string EmojiName { get; }

        public LocalGuildWelcomeScreenChannel(Snowflake channelId, string description, LocalCustomEmoji emoji)
        {
            ChannelId = channelId;
            Description = description;
            EmojiId = emoji.Id;
            EmojiName = emoji.Name;
        }

        public LocalGuildWelcomeScreenChannel(Snowflake channelId, string description, LocalEmoji emoji)
        {
            ChannelId = channelId;
            Description = description;
            EmojiName = emoji.Name;
        }

        public LocalGuildWelcomeScreenChannel(Snowflake channelId, string description)
        {
            ChannelId = channelId;
            Description = description;
        }
    }
}
