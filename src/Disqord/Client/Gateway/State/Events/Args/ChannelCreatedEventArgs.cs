namespace Disqord.Events
{
    public sealed class ChannelCreatedEventArgs : DiscordEventArgs
    {
        public CachedChannel Channel { get; }

        internal ChannelCreatedEventArgs(CachedChannel channel) : base(channel.Client)
        {
            Channel = channel;
        }
    }
}
