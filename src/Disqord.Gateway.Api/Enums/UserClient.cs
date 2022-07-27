using System.Runtime.Serialization;
using Disqord.Serialization.Json;

namespace Disqord;

[StringEnum]
public enum UserClient
{
    [EnumMember(Value = "desktop")]
    Desktop,

    [EnumMember(Value = "mobile")]
    Mobile,

    [EnumMember(Value = "web")]
    Web,

    [EnumMember(Value = "embedded")]
    Embedded
}
