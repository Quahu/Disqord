using System;

namespace Disqord.Serialization.Json;

[AttributeUsage(AttributeTargets.Property)]
public class JsonIgnoreAttribute : Attribute
{
    public JsonIgnoreAttribute()
    { }
}
