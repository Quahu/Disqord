using System;

namespace Disqord.Serialization.Json;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class JsonPropertyAttribute : Attribute
{
    public string Name { get; }

    public NullValueHandling NullValueHandling { get; }

    public JsonPropertyAttribute(string name, NullValueHandling nullValueHandling = default)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace", nameof(name));

        Name = name;
        NullValueHandling = nullValueHandling;
    }
}