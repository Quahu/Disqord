namespace Disqord.Events
{
    public sealed class MessageDeletedEventArgs : DiscordEventArgs
    {
        public ICachedMessageChannel Channel { get; }

        public SnowflakeOptional<CachedUserMessage> Message { get; }

        internal MessageDeletedEventArgs(ICachedMessageChannel channel, SnowflakeOptional<CachedUserMessage> message) : base(channel.Client)
        {
            Channel = channel;
            Message = message;
        }
    }
}
