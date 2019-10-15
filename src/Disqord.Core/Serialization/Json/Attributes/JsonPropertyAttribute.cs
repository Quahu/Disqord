using System;

namespace Disqord.Serialization.Json
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class JsonPropertyAttribute : Attribute
    {
        public string Name { get; }

        public NullValueHandling NullValueHandling { get; set; }

        internal JsonPropertyAttribute(string name)
        {
            Name = name;
        }
    }
}
