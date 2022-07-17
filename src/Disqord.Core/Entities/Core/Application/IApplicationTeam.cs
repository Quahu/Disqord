using System.Collections.Generic;

namespace Disqord;

/// <summary>
///     Represents a team of a Discord application.
/// </summary>
public interface IApplicationTeam : ISnowflakeEntity, INamableEntity
{
    /// <summary>
    ///     Gets the icon image hash of this team.
    /// </summary>
    string? IconHash { get; }

    /// <summary>
    ///     Gets the members of this team.
    /// </summary>
    IReadOnlyDictionary<Snowflake, IApplicationTeamMember> Members { get; }

    /// <summary>
    ///     Gets the ID of the owner of this team.
    /// </summary>
    Snowflake OwnerId { get; }
}
