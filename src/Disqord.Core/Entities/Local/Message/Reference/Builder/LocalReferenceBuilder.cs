using System;

namespace Disqord
{
    public sealed class LocalReferenceBuilder : ICloneable
    {
        public Snowflake MessageId { get; set; }

        public Snowflake? ChannelId { get; set; }

        public Snowflake? GuildId { get; set; }

        public bool FailOnInvalid { get; set; }

        public LocalReferenceBuilder()
        { }

        internal LocalReferenceBuilder(LocalReferenceBuilder builder)
        {
            MessageId = builder.MessageId;
            ChannelId = builder.ChannelId;
            GuildId = builder.GuildId;
            FailOnInvalid = builder.FailOnInvalid;
        }

        public LocalReferenceBuilder WithMessageId(Snowflake messageId)
        {
            MessageId = messageId;
            return this;
        }

        public LocalReferenceBuilder WithChannelId(Snowflake? channelId)
        {
            ChannelId = channelId;
            return this;
        }

        public LocalReferenceBuilder WithGuildId(Snowflake? guildId)
        {
            GuildId = guildId;
            return this;
        }

        public LocalReferenceBuilder WithFailOnInvalid(bool failOnInvalid)
        {
            FailOnInvalid = failOnInvalid;
            return this;
        }

        /// <summary>
        ///     Creates a deep copy of this <see cref="LocalReferenceBuilder"/>.
        /// </summary>
        /// <returns>
        ///     A deep copy of this <see cref="LocalReferenceBuilder"/>.
        /// </returns>
        public LocalReferenceBuilder Clone()
            => new LocalReferenceBuilder(this);

        object ICloneable.Clone()
            => Clone();

        public LocalReference Build()
        {
            if (MessageId == default)
                throw new InvalidOperationException("The referenced message ID must be set.");

            return new LocalReference(this);
        }
    }
}
