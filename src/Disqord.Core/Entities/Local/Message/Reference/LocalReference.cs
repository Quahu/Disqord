namespace Disqord
{
    public sealed class LocalReference
    {
        public Snowflake MessageId { get; }

        public Snowflake? ChannelId { get; }

        public Snowflake? GuildId { get; }

        public bool FailOnInvalid { get; }

        internal LocalReference(LocalReferenceBuilder builder)
        {
            MessageId = builder.MessageId;
            ChannelId = builder.ChannelId;
            GuildId = builder.GuildId;
            FailOnInvalid = builder.FailOnInvalid;
        }
    }
}
