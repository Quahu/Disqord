using System;

namespace Disqord.Gateway;

public class RoleCreatedEventArgs : EventArgs
{
    /// <summary>
    ///     Gets the ID of the guild in which the role was created.
    /// </summary>
    public Snowflake GuildId => Role.GuildId;

    /// <summary>
    ///     Gets the ID of the created role.
    /// </summary>
    public Snowflake RoleId => Role.Id;

    /// <summary>
    ///     Gets the created role.
    /// </summary>
    public IRole Role { get; }

    public RoleCreatedEventArgs(
        IRole role)
    {
        Role = role;
    }
}