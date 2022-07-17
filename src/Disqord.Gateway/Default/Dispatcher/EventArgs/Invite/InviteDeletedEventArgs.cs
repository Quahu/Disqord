using System;

namespace Disqord.Gateway;

public class InviteDeletedEventArgs : EventArgs
{
    /// <summary>
    ///     Gets the ID of the guild in which the invite was deleted.
    ///     Returns <see langword="null"/> if the invite was deleted in a private channel.
    /// </summary>
    public Snowflake? GuildId { get; }

    /// <summary>
    ///     Gets the ID of the channel the invite was deleted for.
    /// </summary>
    public Snowflake ChannelId { get; }

    /// <summary>
    ///     Gets the code of the deleted invite.
    /// </summary>
    public string Code { get; }

    public InviteDeletedEventArgs(
        Snowflake? guildId,
        Snowflake channelId,
        string code)
    {
        GuildId = guildId;
        ChannelId = channelId;
        Code = code;
    }
}