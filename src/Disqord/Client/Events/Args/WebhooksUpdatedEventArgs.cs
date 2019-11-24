namespace Disqord.Events
{
    public sealed class WebhooksUpdatedEventArgs : DiscordEventArgs
    {
        public Snowflake GuildId { get; }

        public Snowflake ChannelId { get; }

        internal WebhooksUpdatedEventArgs(DiscordClientBase client, Snowflake guildId, Snowflake channelId) : base(client)
        {
            GuildId = guildId;
            ChannelId = channelId;
        }
    }
}