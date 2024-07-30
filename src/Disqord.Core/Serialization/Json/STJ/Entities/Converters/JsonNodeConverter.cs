using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Disqord.Serialization.Json.System;

internal sealed class JsonNodeConverter : JsonConverter<IJsonNode>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsAssignableTo(typeof(IJsonNode)) && !typeToConvert.IsAssignableTo(typeof(JsonModel));
    }

    public override IJsonNode? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var node = JsonNode.Parse(ref reader);
        return SystemJsonNode.Create(node, options);
    }

    public override void Write(Utf8JsonWriter writer, IJsonNode value, JsonSerializerOptions options)
    {
        if (value is SystemJsonNode systemJsonNode)
        {
            systemJsonNode.Node.WriteTo(writer, options);
        }
        else
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}
