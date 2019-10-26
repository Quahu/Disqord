namespace Disqord.Events
{
    public sealed class AllReactionsRemovedEventArgs : DiscordEventArgs
    {
        public ICachedMessageChannel Channel { get; }

        public OptionalSnowflakeEntity<CachedMessage> Message { get; }

        internal AllReactionsRemovedEventArgs(
            ICachedMessageChannel channel,
            OptionalSnowflakeEntity<CachedMessage> message) : base(channel.Client)
        {
            Channel = channel;
            Message = message;
        }
    }
}
