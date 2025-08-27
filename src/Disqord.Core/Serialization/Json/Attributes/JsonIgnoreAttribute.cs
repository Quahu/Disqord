using System;

namespace Disqord.Serialization.Json;

[AttributeUsage(AttributeTargets.Field)]
public class JsonIgnoreAttribute : Attribute
{
    public JsonIgnoreAttribute()
    { }
}
