using System;

namespace Disqord.Serialization.Json
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class JsonIgnoreAttribute : Attribute
    {
        internal JsonIgnoreAttribute()
        { }
    }
}
