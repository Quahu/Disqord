using System;

namespace Disqord.Gateway
{
    public class ChannelPinsUpdatedEventArgs : EventArgs
    {
        /// <summary>
        ///     The optional guild ID of this update.
        /// </summary>
        public Snowflake? GuildId { get; }

        /// <summary>
        ///     The channel ID of this event.
        /// </summary>
        public Snowflake ChannelId { get; }

        /// <summary>
        ///     Gets the channel of this update.
        ///     Returns <see langword="null"/> if the channel was not cached.
        /// </summary>
        public CachedTextChannel Channel { get; }

        public ChannelPinsUpdatedEventArgs(Snowflake? guildId, Snowflake channelId, CachedTextChannel channel)
        {
            GuildId = guildId;
            ChannelId = channelId;
            Channel = channel;
        }
    }
}
