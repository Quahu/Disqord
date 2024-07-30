using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Disqord.Serialization.Json.System;

internal sealed class NullableConverter : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsValueType && Nullable.GetUnderlyingType(typeToConvert) != null;
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        return Activator.CreateInstance(typeof(NullableConverterImpl<>).MakeGenericType(Nullable.GetUnderlyingType(typeToConvert)!)) as JsonConverter;
    }

    private sealed class NullableConverterImpl<TValue> : JsonConverter<TValue?>
        where TValue : struct
    {
        public override TValue? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            return JsonSerializer.Deserialize<TValue>(ref reader, options);
        }

        public override void Write(Utf8JsonWriter writer, TValue? value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
                return;
            }

            JsonSerializer.Serialize(writer, value.Value, options);
        }
    }
}
