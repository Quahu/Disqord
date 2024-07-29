using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Nodes;
using Qommon;

namespace Disqord.Serialization.Json.System;

/// <summary>
///     Represents a default JSON node.
///     Wraps a <see cref="JsonNode"/>.
/// </summary>
public class SystemJsonNode : IJsonNode
{
    /// <summary>
    ///     Gets the underlying <see cref="JsonNode"/>.
    /// </summary>
    public JsonNode Node { get; }

    /// <summary>
    ///     Gets the underlying serializer options.
    /// </summary>
    public JsonSerializerOptions Options { get; }

    public SystemJsonNode(JsonNode node, JsonSerializerOptions options)
    {
        Node = node;
        Options = options;
    }

    /// <inheritdoc/>
    public T? ToType<T>()
    {
        return Node.Deserialize<T>(Options);
    }

    /// <inheritdoc/>
    public string ToJsonString(JsonFormatting formatting)
    {
        return Node.ToJsonString(new JsonSerializerOptions(Options)
        {
            WriteIndented = formatting == JsonFormatting.Indented
        });
    }

    /// <summary>
    ///     Creates a new <see cref="SystemJsonNode"/> from the specified object.
    /// </summary>
    /// <param name="obj"> The object to create the node for. </param>
    /// <param name="options"> The JSON serializer options. </param>
    /// <returns>
    ///     A JSON node representing the object.
    /// </returns>
    public static IJsonNode? Create(object? obj, JsonSerializerOptions options)
    {
        var node = JsonSerializer.SerializeToNode(obj, options);
        return Create(node, options);
    }

    [return: NotNullIfNotNull("node")]
    internal static IJsonNode? Create(JsonNode? node, JsonSerializerOptions options)
    {
        return node switch
        {
            null => null,
            JsonObject @object => new SystemJsonObject(@object, options),
            JsonArray array => new SystemJsonArray(array, options),
            JsonValue value => new SystemJsonValue(value, options),
            _ => throw new InvalidOperationException("Unknown JSON node type.")
        };
    }

    [return: NotNullIfNotNull("node")]
    internal static JsonNode? GetSystemNode(IJsonNode? node)
    {
        return node != null
            ? Guard.IsAssignableToType<SystemJsonNode>(node).Node
            : null;
    }
}
