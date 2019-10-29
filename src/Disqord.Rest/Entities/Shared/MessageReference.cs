using Disqord.Models;

namespace Disqord
{
    public sealed class MessageReference
    {
        public Snowflake? MessageId { get; internal set; }

        public Snowflake ChannelId { get; internal set; }

        public Snowflake? GuildId { get; internal set; }

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
