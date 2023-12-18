using System.Runtime.Serialization;
using Disqord.Serialization.Json;

namespace Disqord;

/// <summary>
///     Represents the role of team member in an application team.
/// </summary>
[StringEnum]
public enum TeamMemberRole
{
    /// <summary>
    ///     Represents a team administrator.
    /// </summary>
    [EnumMember(Value = "admin")]
    Administrator,

    /// <summary>
    ///     Represents a team developer.
    /// </summary>
    [EnumMember(Value = "developer")]
    Developer,

    /// <summary>
    ///     Represents a read-only team member.
    /// </summary>
    [EnumMember(Value = "read_only")]
    ReadOnly
}
