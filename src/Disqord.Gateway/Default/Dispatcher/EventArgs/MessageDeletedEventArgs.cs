using System;

namespace Disqord.Gateway
{
    public class MessageDeletedEventArgs : EventArgs
    {
        public Snowflake? GuildId { get; }

        public Snowflake ChannelId { get; }

        public Snowflake MessageId { get; }

        public CachedUserMessage Message { get; }

        public MessageDeletedEventArgs(Snowflake? guildId, Snowflake channelId, Snowflake messageId, CachedUserMessage message)
        {
            GuildId = guildId;
            ChannelId = channelId;
            MessageId = messageId;
            Message = message;
        }
    }
}
