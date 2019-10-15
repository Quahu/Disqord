using System;

namespace Disqord.Serialization.Json
{
    [AttributeUsage(AttributeTargets.Enum, AllowMultiple = false)]
    public sealed class StringEnumAttribute : Attribute
    {
        internal StringEnumAttribute()
        { }
    }
}
