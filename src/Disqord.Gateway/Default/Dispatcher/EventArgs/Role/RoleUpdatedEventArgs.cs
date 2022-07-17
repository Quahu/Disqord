using System;

namespace Disqord.Gateway;

public class RoleUpdatedEventArgs : EventArgs
{
    /// <summary>
    ///     Gets the ID of the guild in which the role was updated.
    /// </summary>
    public Snowflake GuildId { get; }

    /// <summary>
    ///     Gets the ID of the updated role.
    /// </summary>
    public Snowflake RoleId => NewRole.Id;

    /// <summary>
    ///     Gets the role in the state before the update occurred.
    ///     Returns <see langword="null"/> if the role was not cached.
    /// </summary>
    public CachedRole? OldRole { get; }

    /// <summary>
    ///     Gets the updated role.
    /// </summary>
    public IRole NewRole { get; }

    public RoleUpdatedEventArgs(
        Snowflake guildId,
        CachedRole? oldRole,
        IRole newRole)
    {
        GuildId = guildId;
        OldRole = oldRole;
        NewRole = newRole;
    }
}
