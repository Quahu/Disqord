using System;
using Qommon;

namespace Disqord.Gateway;

public class ChannelCreatedEventArgs : EventArgs
{
    /// <summary>
    ///     Gets the ID of the guild the channel was created in.
    /// </summary>
    public Snowflake GuildId => Channel.GuildId;

    /// <summary>
    ///     Gets the ID of the created channel.
    /// </summary>
    public Snowflake ChannelId => Channel.Id;

    /// <summary>
    ///     Gets the created channel.
    /// </summary>
    public IGuildChannel Channel { get; }

    public ChannelCreatedEventArgs(
        IGuildChannel channel)
    {
        Guard.IsNotNull(channel);

        Channel = channel;
    }
}