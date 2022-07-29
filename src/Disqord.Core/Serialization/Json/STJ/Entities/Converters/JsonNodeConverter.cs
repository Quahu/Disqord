using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Disqord.Serialization.Json.STJ.Nodes;

namespace Disqord.Serialization.Json.STJ.Converters;

internal class JsonNodeConverter : JsonConverter<IJsonNode>
{
    public override IJsonNode? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var node = JsonNode.Parse(ref reader);
        return STJJsonNode.Create(node, options);
    }

    public override void Write(Utf8JsonWriter writer, IJsonNode value, JsonSerializerOptions options)
    {
        if (value is STJJsonNode stjNode)
        {
            stjNode.Token.WriteTo(writer, options);
        }
        else
        {
            JsonSerializer.Serialize(writer, value, options);
        }
    }
}