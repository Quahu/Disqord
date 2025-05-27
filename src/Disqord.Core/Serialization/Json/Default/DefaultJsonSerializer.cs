using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Disqord.Serialization.Json.Default;

/// <summary>
///     Represents a System.Text.Json implementation of <see cref="IJsonSerializer"/>.
/// </summary>
public sealed class DefaultJsonSerializer : IJsonSerializer
{
    /// <summary>
    ///     Gets the underlying <see cref="JsonSerializerOptions"/>.
    /// </summary>
    internal JsonSerializerOptions UnderlyingOptions { get; }

    public DefaultJsonSerializer()
    {
        UnderlyingOptions = new JsonSerializerOptions
        {
            NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.AllowNamedFloatingPointLiterals,
            IncludeFields = true,
            IgnoreReadOnlyFields = true,
            IgnoreReadOnlyProperties = true,
            RespectNullableAnnotations = true,
            NewLine = "\n",
            UnknownTypeHandling = JsonUnknownTypeHandling.JsonNode,
            TypeInfoResolver = new JsonTypeInfoResolver(),
            Converters =
            {
                new EnumConverter(),
                new JsonNodeConverter(),
                new NullableConverter(),
                new OptionalConverter(),
                new StringConverter(),
                new SnowflakeConverter(),
                new StreamConverter(),
            }
        };

        UnderlyingOptions.MakeReadOnly();
    }

    /// <inheritdoc/>
    public TValue? Deserialize<TValue>(Stream stream)
    {
        try
        {
            return JsonSerializer.Deserialize<TValue>(stream, UnderlyingOptions);
        }
        catch (JsonException ex)
        {
            ThrowSerializationException(isDeserialize: true, typeof(TValue), ex);
            return default;
        }
    }

    /// <inheritdoc/>
    public void Serialize(Stream stream, object obj, JsonSerializationOptions? options = null)
    {
        try
        {
            var serializerOptions = UnderlyingOptions;
            if (options != null && options.Formatting == JsonFormatting.Indented)
            {
                serializerOptions = new JsonSerializerOptions(UnderlyingOptions);
                serializerOptions.WriteIndented = true;
            }

            JsonSerializer.Serialize(stream, obj, serializerOptions);
        }
        catch (JsonException ex)
        {
            ThrowSerializationException(isDeserialize: false, obj.GetType(), ex);
        }
    }

    /// <inheritdoc/>
    [return: NotNullIfNotNull(nameof(obj))]
    public IJsonNode? GetJsonNode(object? obj)
    {
        return DefaultJsonNode.Create(obj, UnderlyingOptions);
    }

    internal static void ThrowSerializationException(bool isDeserialize, Type type, Exception exception)
    {
        throw new JsonSerializationException(isDeserialize, type, exception);
    }
}
