using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Disqord.Serialization.Json.System;

internal class JsonNodeConverter : JsonConverter<IJsonNode>
{
    public override IJsonNode? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var node = JsonNode.Parse(ref reader);
        return SystemJsonNode.Create(node, options);
    }

    public override void Write(Utf8JsonWriter writer, IJsonNode value, JsonSerializerOptions options)
    {
        if (value is SystemJsonNode systemJsonNode)
        {
            systemJsonNode.Token.WriteTo(writer, options);
        }
        else
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}
