using System;

namespace Disqord.Gateway;

public class BanCreatedEventArgs : EventArgs
{
    /// <summary>
    ///     Gets the ID of the guild the ban was created in.
    /// </summary>
    public Snowflake GuildId { get; }

    /// <summary>
    ///     Gets the ID of the user the ban was created for.
    /// </summary>
    public Snowflake UserId => User.Id;

    /// <summary>
    ///     Gets the user the ban was created for.
    /// </summary>
    public IUser User { get; }

    public BanCreatedEventArgs(
        Snowflake guildId,
        IUser user)
    {
        GuildId = guildId;
        User = user;
    }
}