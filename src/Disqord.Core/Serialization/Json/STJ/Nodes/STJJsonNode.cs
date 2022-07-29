using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Disqord.Serialization.Json.STJ.Nodes;

public class STJJsonNode : IJsonNode
{
    public STJJsonNode(JsonNode node)
    {
        Token = node;
    }

    public JsonNode Token { get; set; }

    public T? ToType<T>()
    {
        return Token.GetValue<T>();
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
            JsonArray jsonArray => new STJJsonArray(jsonArray, options),
            JsonObject jsonObject => new STJJsonObject(jsonObject, options),
            JsonValue jsonValue => new STJJsonValue(jsonValue),
            _ => throw new InvalidOperationException("Unknown JSON token type.")
        };
    }
}