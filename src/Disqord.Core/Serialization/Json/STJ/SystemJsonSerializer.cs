using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

namespace Disqord.Serialization.Json.System;

public class SystemJsonSerializer : IJsonSerializer
{
    public ILogger Logger { get; }

    /// <summary>
    ///     Gets the underlying <see cref="JsonSerializerOptions"/>.
    /// </summary>
    public JsonSerializerOptions UnderlyingOptions { get; }

    public SystemJsonSerializer(ILogger<SystemJsonSerializer> logger)
    {
        Logger = logger;
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

    //TODO create async version
    public object? Deserialize(Stream stream, Type type)
    {
        try
        {
            return JsonSerializer.Deserialize(stream, type, UnderlyingOptions);
        }
        catch (Exception ex)
        {
            throw new JsonSerializationException(true, type, ex);
        }
    }

    //TODO create async version
    public void Serialize(Stream stream, object obj, IJsonSerializerOptions? options = null)
    {
        try
        {
            if (options != null && options.Formatting == JsonFormatting.Indented)
            {
                var serializerOptions = new JsonSerializerOptions(UnderlyingOptions);
                serializerOptions.WriteIndented = true;
                JsonSerializer.Serialize(stream, obj, serializerOptions);
            }
            else
            {
                JsonSerializer.Serialize(stream, obj, UnderlyingOptions);
            }
        }
        catch (Exception ex)
        {
            throw new JsonSerializationException(false, obj.GetType(), ex);
        }
    }

    public IJsonNode GetJsonNode(object? obj)
    {
        return SystemJsonNode.Create(obj, UnderlyingOptions)!;
    }
}
