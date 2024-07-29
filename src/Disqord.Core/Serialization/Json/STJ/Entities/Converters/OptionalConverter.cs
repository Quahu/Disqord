using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Qommon;

namespace Disqord.Serialization.Json.System;

internal class OptionalConverter<TValue> : JsonConverter<Optional<TValue?>>
{
    public override Optional<TValue?> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return JsonSerializer.Deserialize<TValue>(ref reader, options);
    }

    public override void Write(Utf8JsonWriter writer, Optional<TValue?> value, JsonSerializerOptions options)
    {
        var optionalValue = value.Value;
        if (optionalValue == null)
        {
            writer.WriteNullValue();
        }
        else
        {
            JsonSerializer.Serialize(writer, value.Value, options);
        }
    }
}

internal class OptionalConverterWithValueConverter<TValue> : JsonConverter<Optional<TValue?>>
{
    private readonly JsonConverter<TValue?> _valueConverter;

    public OptionalConverterWithValueConverter(JsonConverter<TValue?> valueConverter)
    {
        _valueConverter = valueConverter;
    }

    public override Optional<TValue?> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return _valueConverter.Read(ref reader, typeToConvert, options);
    }

    public override void Write(Utf8JsonWriter writer, Optional<TValue?> value, JsonSerializerOptions options)
    {
        _valueConverter.Write(writer, value.Value, options);
    }
}
