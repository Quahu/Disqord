using System.Runtime.Serialization;
using Disqord.Serialization.Json;

namespace Disqord;

/// <summary>
///     Represents the target of an overwrite.
/// </summary>
[StringEnum]
public enum OverwriteTargetType : byte
{
    /// <summary>
    ///     The overwrite targets a role.
    /// </summary>
    [EnumMember(Value = "role")]
    Role = 0,

    /// <summary>
    ///     The overwrite targets a member.
    /// </summary>
    [EnumMember(Value = "member")]
    Member = 1
}
