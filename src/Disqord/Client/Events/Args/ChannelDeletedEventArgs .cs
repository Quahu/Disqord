namespace Disqord.Events
{
    public sealed class ChannelDeletedEventArgs : DiscordEventArgs
    {
        public CachedChannel Channel { get; }

        internal ChannelDeletedEventArgs(CachedChannel channel) : base(channel.Client)
        {
            Channel = channel;
        }
    }
}
