using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Disqord.Serialization.Json.Default;

public class SnowflakeDictionaryConverter<TValue> : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return true;
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
            return null;

        if (JToken.ReadFrom(reader) is not JObject jObject)
            throw new InvalidOperationException("Not a JSON object.");

        var dictionary = new Dictionary<Snowflake, TValue>(jObject.Count);
        foreach (var kvp in jObject)
            dictionary.Add(Convert.ToUInt64(kvp.Key), (kvp.Value != null ? kvp.Value.ToObject<TValue>(serializer) : default)!);

        return dictionary;
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        var dictionary = (value as Dictionary<Snowflake, TValue>)!;
        writer.WriteStartObject();
        foreach (var kvp in dictionary)
        {
            writer.WritePropertyName(kvp.Key.ToString());
            if (kvp.Value == null)
            {
                writer.WriteNull();
            }
            else
            {
                JToken.FromObject(kvp.Value, serializer).WriteTo(writer);
            }
        }

        writer.WriteEndObject();
    }
}
