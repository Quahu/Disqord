namespace Disqord.Events
{
    public sealed class MessageUpdatedEventArgs : DiscordEventArgs
    {
        public ICachedMessageChannel Channel { get; }

        public SnowflakeOptional<CachedUserMessage> OldMessage { get; }

        public CachedUserMessage NewMessage { get; }

        internal MessageUpdatedEventArgs(ICachedMessageChannel channel, SnowflakeOptional<CachedUserMessage> oldMessage, CachedUserMessage newMessage)
            : base(channel.Client)
        {
            Channel = channel;
            OldMessage = oldMessage;
            NewMessage = newMessage;
        }
    }
}
