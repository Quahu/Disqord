using System.Runtime.Serialization;
using Disqord.Serialization.Json;

namespace Disqord
{
    [StringEnum]
    public enum Theme : byte
    {
        [EnumMember(Value = "dark")]
        Dark,

        [EnumMember(Value = "light")]
        Light
    }
}
