using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Disqord.Serialization.Json.System;

public class SystemJsonSerializer : IJsonSerializer
{
    /// <summary>
    ///     Gets the underlying <see cref="JsonSerializerOptions"/>.
    /// </summary>
    internal JsonSerializerOptions UnderlyingOptions { get; }

    public SystemJsonSerializer()
    {
        UnderlyingOptions = new JsonSerializerOptions
        {
            NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.AllowNamedFloatingPointLiterals,
            IncludeFields = true,
            IgnoreReadOnlyFields = true,
            IgnoreReadOnlyProperties = true,
            TypeInfoResolver = new JsonTypeInfoResolver(),
            Converters = { new EnumConverter(), new SnowflakeConverter(), new StreamConverter() }
        };
    }

    /// <inheritdoc/>
    public object? Deserialize(Stream stream, Type type)
    {
        try
        {
            return JsonSerializer.Deserialize(stream, type, UnderlyingOptions);
        }
        catch (JsonException ex)
        {
            throw new JsonSerializationException(isDeserialize: true, type, ex);
        }
    }

    /// <inheritdoc/>
    public void Serialize(Stream stream, object obj, IJsonSerializerOptions? options = null)
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
            throw new JsonSerializationException(isDeserialize: false, obj.GetType(), ex);
        }
    }

    /// <inheritdoc/>
    [return: NotNullIfNotNull(nameof(obj))]
    public IJsonNode? GetJsonNode(object? obj)
    {
        return SystemJsonNode.Create(obj, UnderlyingOptions);
    }
}
