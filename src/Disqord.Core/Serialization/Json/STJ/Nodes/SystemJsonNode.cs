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
internal abstract class SystemJsonNode : IJsonNode
{
    /// <summary>
    ///     Gets the underlying <see cref="JsonNode"/>.
    /// </summary>
    public JsonNode Node { get; }

    /// <summary>
    ///     Gets the underlying serializer options.
    /// </summary>
    public JsonSerializerOptions Options { get; }

    /// <inheritdoc/>
    public string Path => Node.GetPath();

    /// <inheritdoc/>
    public JsonValueType Type => Node.GetValueKind() switch
    {
        JsonValueKind.Object => JsonValueType.Object,
        JsonValueKind.Array => JsonValueType.Array,
        JsonValueKind.String => JsonValueType.String,
        JsonValueKind.Number => JsonValueType.Number,
        JsonValueKind.True => JsonValueType.True,
        JsonValueKind.False => JsonValueType.False,
        _ => JsonValueType.Null
    };

    private protected SystemJsonNode(JsonNode node, JsonSerializerOptions options)
    {
        Node = node;
        Options = options;
    }

    /// <inheritdoc/>
    public T? ToType<T>()
    {
        try
        {
            var value = Node.Deserialize<T>(Options);
            if (typeof(T) != typeof(JsonElement) && value is JsonElement)
            {
                Throw.ArgumentException($"Cannot convert the value to type {typeof(T)}.");
            }

            return value;
        }
        catch (JsonException ex)
        {
            SystemJsonSerializer.ThrowSerializationException(isDeserialize: true, typeof(T), ex);
            return default;
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
            SystemJsonSerializer.ThrowSerializationException(isDeserialize: false, obj?.GetType() ?? typeof(object), ex);
            return null;
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
