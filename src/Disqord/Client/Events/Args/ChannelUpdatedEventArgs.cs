namespace Disqord.Events
{
    public sealed class ChannelUpdatedEventArgs : DiscordEventArgs
    {
        public CachedChannel OldChannel { get; }

        public CachedChannel NewChannel { get; }

        internal ChannelUpdatedEventArgs(CachedChannel oldChannel, CachedChannel newChannel) : base(newChannel.Client)
        {
            OldChannel = oldChannel;
            NewChannel = newChannel;
        }
    }
}
