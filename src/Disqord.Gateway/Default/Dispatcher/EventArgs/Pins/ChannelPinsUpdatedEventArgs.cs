using System;

namespace Disqord.Gateway;

public class ChannelPinsUpdatedEventArgs : EventArgs
{
    /// <summary>
    ///     Gets the ID of the guild the pins were updated in.
    ///     Returns <see langword="null"/> if the pins were updated in a private channel.
    /// </summary>
    public Snowflake? GuildId { get; }

    /// <summary>
    ///     Gets the ID of the channel the pins were updated in.
    /// </summary>
    public Snowflake ChannelId { get; }

    /// <summary>
    ///     Gets the channel the pins were updated in.
    ///     Returns <see langword="null"/> if the channel was not cached or the pins were updated in a private channel.
    /// </summary>
    public CachedMessageGuildChannel? Channel { get; }

    public ChannelPinsUpdatedEventArgs(
        Snowflake? guildId,
        Snowflake channelId,
        CachedMessageGuildChannel? channel)
    {
        GuildId = guildId;
        ChannelId = channelId;
        Channel = channel;
    }
}
