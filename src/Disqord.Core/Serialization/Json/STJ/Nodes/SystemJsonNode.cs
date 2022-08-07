using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Disqord.Serialization.Json.System;

public class SystemJsonNode : IJsonNode
{
    protected readonly JsonSerializerOptions Options;

    public SystemJsonNode(JsonNode node, JsonSerializerOptions options)
    {
        Token = node;
        Options = options;
    }

    public JsonNode Token { get; set; }

    public T? ToType<T>()
    {
        return Token.Deserialize<T>(Options);
    }

    public static IJsonNode? Create(object? obj, JsonSerializerOptions options)
    {
        var element = JsonSerializer.SerializeToNode(obj, options);
        return Create(element, options);
    }

    [return: NotNullIfNotNull("token")]
    internal static IJsonNode? Create(JsonNode? token, JsonSerializerOptions options)
    {
        return token switch
        {
            null => null,
            JsonArray jsonArray => new SystemJsonArray(jsonArray, options),
            JsonObject jsonObject => new SystemJsonObject(jsonObject, options),
            JsonValue jsonValue => new SystemJsonValue(jsonValue, options),
            _ => throw new InvalidOperationException("Unknown JSON token type.")
        };
    }
}
