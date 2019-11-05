using Disqord.Models;

namespace Disqord
{
    public sealed class MessageReference
    {
        public Snowflake? MessageId { get; }

        public Snowflake ChannelId { get; }

        public Snowflake? GuildId { get; }

        internal MessageReference(MessageReferenceModel model)
        {
            if (model.MessageId.HasValue)
                MessageId = model.MessageId.Value;

            ChannelId = model.ChannelId;

            if (model.MessageId.HasValue)
                GuildId = model.GuildId.Value;
        }
    }
}
