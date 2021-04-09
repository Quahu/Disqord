using System;

namespace Disqord.Gateway
{
    public class ChannelDeletedEventArgs : EventArgs
    {
        /// <summary>
        ///     Gets the ID of the deleted channel.
        /// </summary>
        public Snowflake ChannelId => Channel.Id;

        /// <summary>
        ///     Gets the deleted channel.
        /// </summary>
        public IGuildChannel Channel { get; }

        public ChannelDeletedEventArgs(IGuildChannel channel)
        {
            if (channel == null)
                throw new ArgumentNullException(nameof(channel));

            Channel = channel;
        }
    }
}
