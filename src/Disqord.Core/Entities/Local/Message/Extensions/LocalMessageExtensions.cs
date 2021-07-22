namespace Disqord
{
    public static class LocalMessageExtensions
    {
        public static TMessage WithReference<TMessage>(this TMessage message, LocalMessageReference reference)
            where TMessage : LocalMessage
        {
            message.Reference = reference;
            return message;
        }

        public static TMessage WithReply<TMessage>(this TMessage message, Snowflake messageId, Snowflake? channelId = null, Snowflake? guildId = null, bool failOnUnknownMessage = false)
            where TMessage : LocalMessage
        {
            var reference = message.Reference ??= new LocalMessageReference();
            reference.MessageId = messageId;
            reference.ChannelId = channelId;
            reference.GuildId = guildId;
            reference.FailOnUnknownMessage = failOnUnknownMessage;
            return message;
        }

        public static TMessage WithNonce<TMessage>(this TMessage message, string nonce)
            where TMessage : LocalMessage
        {
            message.Nonce = nonce;
            return message;
        }
    }
}
