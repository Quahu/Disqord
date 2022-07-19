using System;

namespace Disqord.Serialization.Json;

[AttributeUsage(AttributeTargets.Enum)]
public class StringEnumAttribute : Attribute
{
    public StringEnumAttribute()
    { }
}
