using Disqord.Models;

namespace Disqord;

/// <summary>
///     Represents an application command permission.
/// </summary>
public interface IApplicationCommandPermission : IEntity, IJsonUpdatable<ApplicationCommandPermissionsJsonModel>
{
    /// <summary>
    ///     Gets the ID of the target of this permission.
    /// </summary>
    /// <remarks>
    ///     May also return the ID of the guild to target all members,
    ///     or (ID of the guild - 1) to target all channels in the guild.
    /// </remarks>
    Snowflake TargetId { get; }

    /// <summary>
    ///     Gets the type of the target of this permission.
    /// </summary>
    ApplicationCommandPermissionTargetType TargetType { get; }

    /// <summary>
    ///     Gets whether the target entity has permission to execute the command.
    /// </summary>
    bool HasPermission { get; }
}