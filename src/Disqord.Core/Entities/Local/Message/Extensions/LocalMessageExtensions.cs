namespace Disqord
{
    public static class LocalMessageExtensions
    {
        public static TLocalMessage WithReference<TLocalMessage>(this TLocalMessage message, LocalMessageReference reference)
            where TLocalMessage : LocalMessage
        {
            message.Reference = reference;
            return message;
        }

        public static TLocalMessage WithReply<TLocalMessage>(this TLocalMessage message, Snowflake messageId, Snowflake? channelId = null, Snowflake? guildId = null, bool failOnInvalid = true)
            where TLocalMessage : LocalMessage
        {
            var reference = message.Reference ??= new LocalMessageReference();
            reference.MessageId = messageId;
            reference.ChannelId = channelId;
            reference.GuildId = guildId;
            reference.FailOnInvalid = failOnInvalid;
            return message;
        }

        public static TLocalMessage WithNonce<TLocalMessage>(this TLocalMessage message, string nonce)
            where TLocalMessage : LocalMessage
        {
            message.Nonce = nonce;
            return message;
        }
    }
}
