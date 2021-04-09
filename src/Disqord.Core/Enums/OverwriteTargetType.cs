using System.Runtime.Serialization;
using Disqord.Serialization.Json;

namespace Disqord
{
    [StringEnum]
    public enum OverwriteTargetType : byte
    {
        [EnumMember(Value = "role")]
        Role,

        [EnumMember(Value = "member")]
        Member
    }
}
