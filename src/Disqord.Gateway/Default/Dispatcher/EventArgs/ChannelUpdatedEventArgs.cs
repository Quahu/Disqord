using System;

namespace Disqord.Gateway
{
    public class ChannelUpdatedEventArgs : EventArgs
    {
        /// <summary>
        ///     Gets the channel in the state before the update occurred.
        ///     Returns <see langword="null"/> if the channel was not cached.
        /// </summary>
        public IGuildChannel OldChannel { get; }

        /// <summary>
        ///     Gets the updated channel.
        /// </summary>
        public IGuildChannel NewChannel { get; }

        public ChannelUpdatedEventArgs(IGuildChannel oldChannel, IGuildChannel newChannel)
        {
            OldChannel = oldChannel;
            NewChannel = newChannel;
        }
    }
}
