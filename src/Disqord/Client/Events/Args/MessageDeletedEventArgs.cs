namespace Disqord.Events
{
    public sealed class MessageDeletedEventArgs : DiscordEventArgs
    {
        public ICachedMessageChannel Channel { get; }

        public OptionalSnowflakeEntity<CachedUserMessage> Message { get; }

        internal MessageDeletedEventArgs(ICachedMessageChannel channel, OptionalSnowflakeEntity<CachedUserMessage> message) : base(channel.Client)
        {
            Channel = channel;
            Message = message;
        }
    }
}
