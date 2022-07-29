using System;
using System.IO;
using System.Text.Json;
using Disqord.Serialization.Json.STJ.Converters;
using Disqord.Serialization.Json.STJ.Entities;
using Disqord.Serialization.Json.STJ.Nodes;
using Microsoft.Extensions.Logging;

namespace Disqord.Serialization.Json.STJ;

public class SystemTextJsonSerializer : IJsonSerializer
{
    public ILogger Logger { get; }


    private readonly JsonSerializerOptions _options;

    public SystemTextJsonSerializer(ILogger<SystemTextJsonSerializer> logger, SystemTextJsonSerializerOptions configuration)
    {
        Logger = logger;
        _options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            WriteIndented = configuration.Formatting == JsonFormatting.Indented,
            TypeInfoResolver = new CustomResolver(),
            Converters = { new JsonNodeConverter(), new OptionalConverter(), new SnowflakeConverter(), new StreamConverter(this) }
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
            JsonSerializer.Serialize(stream, obj);
        }
        catch (Exception ex)
        {
            throw new JsonSerializationException(false, obj.GetType(), ex);
        }
    }

    public IJsonNode GetJsonNode(object? obj)
    {
        return STJJsonNode.Create(obj, _options)!;
    }
}