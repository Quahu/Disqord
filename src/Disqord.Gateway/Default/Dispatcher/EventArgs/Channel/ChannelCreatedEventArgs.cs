using System;

namespace Disqord.Gateway
{
    public class ChannelCreatedEventArgs : EventArgs
    {
        /// <summary>
        ///     Gets the ID of the created channel.
        /// </summary>
        public Snowflake ChannelId => Channel.Id;

        /// <summary>
        ///     Gets the created channel.
        /// </summary>
        public IGuildChannel Channel { get; }

        public ChannelCreatedEventArgs(IGuildChannel channel)
        {
            if (channel == null)
                throw new ArgumentNullException(nameof(channel));

            Channel = channel;
        }
    }
}
