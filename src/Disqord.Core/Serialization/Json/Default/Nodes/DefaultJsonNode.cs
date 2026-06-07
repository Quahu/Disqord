using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Nodes;
using Qommon;

namespace Disqord.Serialization.Json.Default;

/// <summary>
///     Represents a default JSON node.
///     Wraps a <see cref="JsonNode"/>.
/// </summary>
internal abstract class DefaultJsonNode : IJsonNode
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

    private protected DefaultJsonNode(JsonNode node, JsonSerializerOptions options)
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
            if (value is JsonElement element && typeof(T) != typeof(JsonElement))
            {
                return (T) GetClrValue(element);
            }

            if (value is JsonValue jsonValue && typeof(T) != typeof(JsonNode) && typeof(T) != typeof(JsonValue))
            {
                if (typeof(T) == typeof(object))
                {
                    return (T) GetClrValue(jsonValue.GetValue<JsonElement>());
                }

                return jsonValue.GetValue<T>();
            }

            return value;
        }
        catch (JsonException ex)
        {
            DefaultJsonSerializer.ThrowSerializationException(isDeserialize: true, typeof(T), ex);
            return default;
        }
    }

    private static object GetClrValue(JsonElement element)
    {
        return element.ValueKind switch
        {
            JsonValueKind.String => element.GetString()!,
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Null => null!,
            JsonValueKind.Number when element.TryGetInt32(out var i) => i,
            JsonValueKind.Number when element.TryGetInt64(out var l) => l,
            JsonValueKind.Number when element.TryGetDouble(out var d) => d,
            JsonValueKind.Number when element.TryGetDecimal(out var m) => m,
            _ => element
        };
    }

    /// <inheritdoc/>
    public string ToJsonString(JsonFormatting formatting)
    {
        return Node.ToJsonString(new JsonSerializerOptions(Options)
        {
            WriteIndented = formatting == JsonFormatting.Indented
        });
    }

    /// <inheritdoc/>
    public bool DeepEquals(IJsonNode? other)
    {
        if (other is not DefaultJsonNode otherNode)
        {
            return false;
        }

        return JsonNode.DeepEquals(Node, otherNode.Node);
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
            DefaultJsonSerializer.ThrowSerializationException(isDeserialize: false, obj?.GetType() ?? typeof(object), ex);
            return null;
        }
    }

    [return: NotNullIfNotNull("node")]
    internal static IJsonNode? Create(JsonNode? node, JsonSerializerOptions options)
    {
        return node switch
        {
            null => null,
            JsonObject @object => new DefaultJsonObject(@object, options),
            JsonArray array => new DefaultJsonArray(array, options),
            JsonValue value => new DefaultJsonValue(value, options),
            _ => throw new InvalidOperationException("Unknown JSON node type.")
        };
    }

    [return: NotNullIfNotNull("node")]
    internal static JsonNode? GetSystemNode(IJsonNode? node)
    {
        return node != null
            ? Guard.IsAssignableToType<DefaultJsonNode>(node).Node
            : null;
    }
}
