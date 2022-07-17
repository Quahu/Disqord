using System.Collections.Generic;
using Disqord.Models;

namespace Disqord;

/// <summary>
///     Represents an application command's guild permissions.
/// </summary>
public interface IApplicationCommandGuildPermissions : ISnowflakeEntity, IGuildEntity, IJsonUpdatable<ApplicationCommandGuildPermissionsJsonModel>
{
    /// <summary>
    ///     Gets the ID of the application that the command belongs to.
    /// </summary>
    Snowflake ApplicationId { get; }

    /// <summary>
    ///     Gets the permissions for the command.
    /// </summary>
    IReadOnlyList<IApplicationCommandPermission> Permissions { get; }
}