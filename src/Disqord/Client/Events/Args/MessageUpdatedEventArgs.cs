namespace Disqord.Events
{
    public sealed class MessageUpdatedEventArgs : DiscordEventArgs
    {
        public ICachedMessageChannel Channel { get; }

        public OptionalSnowflakeEntity<CachedUserMessage> OldMessage { get; }

        public CachedUserMessage NewMessage { get; }

        internal MessageUpdatedEventArgs(ICachedMessageChannel channel, OptionalSnowflakeEntity<CachedUserMessage> oldMessage, CachedUserMessage newMessage)
            : base(channel.Client)
        {
            Channel = channel;
            OldMessage = oldMessage;
            NewMessage = newMessage;
        }
    }
}
