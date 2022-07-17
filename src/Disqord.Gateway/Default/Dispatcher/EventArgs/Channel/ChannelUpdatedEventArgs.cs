using System;

namespace Disqord.Gateway;

public class ChannelUpdatedEventArgs : EventArgs
{
    /// <summary>
    ///     Gets the ID of the guild the channel was updated in.
    /// </summary>
    public Snowflake GuildId => NewChannel.GuildId;

    /// <summary>
    ///     Gets the ID of the updated channel.
    /// </summary>
    public Snowflake ChannelId => NewChannel.Id;

    /// <summary>
    ///     Gets the channel in the state before the update occurred.
    ///     Returns <see langword="null"/> if the channel was not cached.
    /// </summary>
    public CachedGuildChannel? OldChannel { get; }

    /// <summary>
    ///     Gets the updated channel.
    /// </summary>
    public IGuildChannel NewChannel { get; }

    public ChannelUpdatedEventArgs(
        CachedGuildChannel? oldChannel,
        IGuildChannel newChannel)
    {
        OldChannel = oldChannel;
        NewChannel = newChannel;
    }
}
