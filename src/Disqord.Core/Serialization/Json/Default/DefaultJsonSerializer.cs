using System;
using System.Diagnostics;
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
                new ComponentConverter(),
                new UnfurledMediaItemConverter(),
            }
        };

        UnderlyingOptions.MakeReadOnly();

        foreach (var converter in UnderlyingOptions.Converters)
        {
            if (converter is not IPolymorphicJsonConverter polymorphicConverter)
            {
                continue;
            }

            const int ObjectConverterStrategy = 0x1;
            converter.SetConverterStrategy(ObjectConverterStrategy);

            var polymorphicOptions = new JsonSerializerOptions(UnderlyingOptions);
            var converterIndex = polymorphicOptions.Converters.IndexOf(converter);
            Debug.Assert(converterIndex != -1);

            polymorphicOptions.Converters.RemoveAt(converterIndex);
            polymorphicOptions.MakeReadOnly();

            // Each polymorphic converter gets its own set of options which don't contain that converter.
            polymorphicConverter.SetOptionsWithoutSelf(polymorphicOptions);
        }
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
