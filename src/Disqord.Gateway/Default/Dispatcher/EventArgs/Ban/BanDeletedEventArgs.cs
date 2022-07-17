using System;

namespace Disqord.Gateway;

public class BanDeletedEventArgs : EventArgs
{
    /// <summary>
    ///     Gets the ID of the guild the ban was deleted in.
    /// </summary>
    public Snowflake GuildId { get; }

    /// <summary>
    ///     Gets the ID of the user the ban was deleted for.
    /// </summary>
    public Snowflake UserId => User.Id;

    /// <summary>
    ///     Gets the user the ban was deleted for.
    /// </summary>
    public IUser User { get; }

    public BanDeletedEventArgs(
        Snowflake guildId,
        IUser user)
    {
        GuildId = guildId;
        User = user;
    }
}