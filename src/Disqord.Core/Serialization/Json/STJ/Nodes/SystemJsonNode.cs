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
        try
        {
            return Node.Deserialize<T>(Options);
        }
        catch (JsonException ex)
        {
            throw new JsonSerializationException(isDeserialize: true, typeof(T), ex);
        }
    }

    /// <inheritdoc/>
    public string ToJsonString(JsonFormatting formatting)
    {
        return Node.ToJsonString(new JsonSerializerOptions(Options)
        {
            WriteIndented = formatting == JsonFormatting.Indented
        });
    }

    [return: NotNullIfNotNull("obj")]
    internal static IJsonNode? Create(object? obj, JsonSerializerOptions options)
    {
        try
        {
            var node = JsonSerializer.SerializeToNode(obj, options);
            return Create(node, options);
        }
        catch (JsonException ex)
        {
            throw new JsonSerializationException(isDeserialize: false, obj?.GetType() ?? typeof(object), ex);
        }
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
