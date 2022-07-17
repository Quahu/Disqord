using System;
using Qommon;

namespace Disqord.Gateway;

public class ChannelDeletedEventArgs : EventArgs
{
    /// <summary>
    ///     Gets the ID of the guild the channel was deleted in.
    /// </summary>
    public Snowflake GuildId => Channel.GuildId;

    /// <summary>
    ///     Gets the ID of the deleted channel.
    /// </summary>
    public Snowflake ChannelId => Channel.Id;

    /// <summary>
    ///     Gets the deleted channel.
    /// </summary>
    public IGuildChannel Channel { get; }

    public ChannelDeletedEventArgs(
        IGuildChannel channel)
    {
        Guard.IsNotNull(channel);

        Channel = channel;
    }
}