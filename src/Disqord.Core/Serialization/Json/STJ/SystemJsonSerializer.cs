using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

namespace Disqord.Serialization.Json.System;

public class SystemJsonSerializer : IJsonSerializer
{
    public ILogger Logger { get; }

    private readonly JsonSerializerOptions _options;

    public SystemJsonSerializer(ILogger<SystemJsonSerializer> logger)
    {
        Logger = logger;
        _options = new JsonSerializerOptions
        {
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            IncludeFields = true,
            IgnoreReadOnlyFields = true,
            IgnoreReadOnlyProperties = true,
            TypeInfoResolver = new JsonTypeInfoResolver(),

            // TODO: proper string enum converter
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase), new JsonNodeConverter(), new SnowflakeConverter(), new StreamConverter(this) }
        };
    }

    //TODO create async version
    public object? Deserialize(Stream stream, Type type)
    {
        try
        {
            return JsonSerializer.Deserialize(stream, type, _options);
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
                var serializerOptions = new JsonSerializerOptions(_options);
                serializerOptions.WriteIndented = true;
                JsonSerializer.Serialize(stream, obj, serializerOptions);
            }
            else
            {
                JsonSerializer.Serialize(stream, obj, _options);
            }
        }
        catch (Exception ex)
        {
            throw new JsonSerializationException(false, obj.GetType(), ex);
        }
    }

    public IJsonNode GetJsonNode(object? obj)
    {
        return SystemJsonNode.Create(obj, _options)!;
    }
}
