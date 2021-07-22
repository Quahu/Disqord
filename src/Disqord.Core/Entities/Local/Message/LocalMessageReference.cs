using System;

namespace Disqord
{
    public class LocalMessageReference : ILocalConstruct
    {
        public Snowflake MessageId { get; set; }

        public Snowflake? ChannelId { get; set; }

        public Snowflake? GuildId { get; set; }

        /// <summary>
        ///     Gets or sets whether the message request should fail if the message is not found.
        /// </summary>
        /// <remarks>
        ///     This does not prevent errors on invalid <see cref="ChannelId"/> and/or <see cref="GuildId"/>.
        /// </remarks>
        public bool FailOnUnknownMessage { get; set; }

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

        public LocalMessageReference WithFailOnUnknownMessage(bool failOnUnknownMessage = true)
        {
            FailOnUnknownMessage = failOnUnknownMessage;
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
