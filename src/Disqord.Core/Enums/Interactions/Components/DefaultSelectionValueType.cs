using System.Runtime.Serialization;
using Disqord.Serialization.Json;

namespace Disqord;

/// <summary>
///     Represents the type of default selection value.
/// </summary>
[StringEnum]
public enum DefaultSelectionValueType
{
    [EnumMember(Value = "user")]
    User,

    [EnumMember(Value = "role")]
    Role,

    [EnumMember(Value = "channel")]
    Channel,
}
