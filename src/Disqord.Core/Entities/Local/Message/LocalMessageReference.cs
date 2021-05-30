using System;

namespace Disqord
{
    public sealed class LocalMessageReference : ICloneable
    {
        public Snowflake MessageId { get; set; }

        public Snowflake? ChannelId { get; set; }

        public Snowflake? GuildId { get; set; }

        public bool FailOnInvalid { get; set; }

        public LocalMessageReference()
        { }

        public LocalMessageReference WithMessageId(Snowflake messageId)
        {
            MessageId = messageId;
            return this;
        }

        public LocalMessageReference WithChannelId(Snowflake? channelId)
        {
            ChannelId = channelId;
            return this;
        }

        public LocalMessageReference WithGuildId(Snowflake? guildId)
        {
            GuildId = guildId;
            return this;
        }

        public LocalMessageReference WithFailOnInvalid(bool failOnInvalid)
        {
            FailOnInvalid = failOnInvalid;
            return this;
        }

        /// <summary>
        ///     Creates a deep copy of this message reference.
        /// </summary>
        /// <returns>
        ///     A deep copy of this message reference.
        /// </returns>
        public LocalMessageReference Clone()
            => MemberwiseClone() as LocalMessageReference;

        object ICloneable.Clone()
            => Clone();

        public void Validate()
        {
            if (MessageId == default)
                throw new InvalidOperationException("The referenced message ID must be set.");
        }
    }
}
