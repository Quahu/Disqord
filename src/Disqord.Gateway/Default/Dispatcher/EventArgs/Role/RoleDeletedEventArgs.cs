using System;

namespace Disqord.Gateway;

public class RoleDeletedEventArgs : EventArgs
{
    /// <summary>
    ///     Gets the ID of the guild in which the role was deleted.
    /// </summary>
    public Snowflake GuildId { get; }

    /// <summary>
    ///     Gets the ID of the deleted role.
    /// </summary>
    public Snowflake RoleId { get; }

    /// <summary>
    ///     Gets the role in the state before the delete occurred.
    ///     Returns <see langword="null"/> if the role was not cached.
    /// </summary>
    public CachedRole? Role { get; }

    public RoleDeletedEventArgs(
        Snowflake guildId,
        Snowflake roleId,
        CachedRole? role)
    {
        GuildId = guildId;
        RoleId = roleId;
        Role = role;
    }
}
