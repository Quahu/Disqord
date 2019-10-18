namespace Disqord
{
    public sealed class ModifyWebhookProperties
    {
        public Optional<string> Name { internal get; set; }

        public Optional<LocalAttachment> Avatar { internal get; set; }

        public Optional<Snowflake> ChannelId { internal get; set; }

        internal ModifyWebhookProperties()
        { }

        internal bool HasValues
            => Name.HasValue || Avatar.HasValue || ChannelId.HasValue;
    }
}
