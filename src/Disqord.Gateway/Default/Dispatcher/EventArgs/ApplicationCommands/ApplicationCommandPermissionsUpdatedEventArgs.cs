using System;

namespace Disqord.Gateway;

public class ApplicationCommandPermissionsUpdatedEventArgs : EventArgs
{
    /// <summary>
    ///     Gets the ID of the guild which the permissions were updated in.
    /// </summary>
    public Snowflake GuildId => Permissions.GuildId;

    /// <summary>
    ///     Gets the updated guild permissions.
    /// </summary>
    public IApplicationCommandGuildPermissions Permissions { get; }

    public ApplicationCommandPermissionsUpdatedEventArgs(
        IApplicationCommandGuildPermissions permissions)
    {
        Permissions = permissions;
    }
}
