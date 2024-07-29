using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using BufferType =
#if NET8_0_OR_GREATER
    byte
#else
    char
#endif
    ;

namespace Disqord.Serialization.Json.System;

public class SnowflakeDictionaryConverter<TValue> : JsonConverter<Dictionary<Snowflake, TValue>>
{
    public override Dictionary<Snowflake, TValue>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        var jsonObject = JsonSerializer.Deserialize<JsonObject>(ref reader, options)!;
        var count = jsonObject.Count;
        var dictionary = new Dictionary<Snowflake, TValue>(count);
        for (var i = 0; i < count; i++)
        {
            var property = jsonObject.GetAt(i);
            dictionary.Add(
                Convert.ToUInt64(property.Key, CultureInfo.InvariantCulture),
                (property.Value != null
                    ? property.Value.Deserialize<TValue>(options)
                    : default)!);
        }

        return dictionary;
    }

    public override void Write(Utf8JsonWriter writer, Dictionary<Snowflake, TValue> value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        var buffer = (stackalloc BufferType[20]);
        foreach (var kvp in value)
        {
            kvp.Key.RawValue.TryFormat(buffer, out var countWritten);
            writer.WritePropertyName(buffer[..countWritten]);

            if (kvp.Value == null)
            {
                writer.WriteNullValue();
            }
            else
            {
                JsonSerializer.Serialize(writer, kvp.Value, options);
            }
        }

        writer.WriteEndObject();
    }
}
