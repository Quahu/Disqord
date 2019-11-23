using System;

namespace Disqord.Serialization.Json
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class JsonPropertyAttribute : Attribute
    {
        public string Name { get; }

        public NullValueHandling NullValueHandling { get; }

        internal JsonPropertyAttribute(string name, NullValueHandling nullValueHandling = NullValueHandling.Include)
        {
            Name = name;
            NullValueHandling = nullValueHandling;
        }
    }
}
