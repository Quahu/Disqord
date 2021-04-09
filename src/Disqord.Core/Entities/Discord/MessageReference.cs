using Disqord.Models;

namespace Disqord
{
    public class MessageReference
    {
        public Snowflake? MessageId { get; }

        public Snowflake ChannelId { get; }

        public Snowflake? GuildId { get; }

        public MessageReference(MessageReferenceJsonModel model)
        {
            MessageId = model.MessageId.GetValueOrNullable();
            ChannelId = model.ChannelId.Value;
            GuildId = model.GuildId.GetValueOrNullable();
        }
    }
}
