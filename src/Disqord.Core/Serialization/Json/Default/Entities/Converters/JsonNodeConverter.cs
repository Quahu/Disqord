using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Disqord.Serialization.Json.Default;

internal sealed class JsonNodeConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return true;
    }

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var token = JToken.ReadFrom(reader);
        return DefaultJsonNode.Create(token, serializer);
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        var token = value is DefaultJsonNode defaultJsonNode
            ? defaultJsonNode.Token
            : JToken.FromObject(value!, serializer);

        serializer.Serialize(writer, token);
    }
}
