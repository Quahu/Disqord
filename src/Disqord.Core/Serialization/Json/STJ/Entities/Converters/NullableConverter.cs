using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Disqord.Serialization.Json.System;

public class NullableConverter<TValue> : JsonConverter<TValue?>
    where TValue : struct
{
    private readonly JsonConverter<TValue> _valueConverter;

    public NullableConverter(JsonConverter<TValue> valueConverter)
    {
        _valueConverter = valueConverter;
    }

    public override TValue? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        return _valueConverter.Read(ref reader, typeToConvert, options);
    }

    public override void Write(Utf8JsonWriter writer, TValue? value, JsonSerializerOptions options)
    {
        if (value == null)
        {
            writer.WriteNullValue();
            return;
        }

        _valueConverter.Write(writer, value.Value, options);
    }
}
