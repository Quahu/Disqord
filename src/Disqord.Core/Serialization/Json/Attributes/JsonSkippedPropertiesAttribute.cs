using System;

namespace Disqord.Serialization.Json;

[AttributeUsage(AttributeTargets.Class)]
public sealed class JsonSkippedPropertiesAttribute : Attribute
{
    public string[] Properties { get; }

    public JsonSkippedPropertiesAttribute(params string[] properties)
    {
        Properties = properties;
    }
}