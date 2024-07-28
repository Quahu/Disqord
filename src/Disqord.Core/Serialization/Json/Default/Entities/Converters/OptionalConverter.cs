using System;
using Newtonsoft.Json;
using Qommon;
using Qommon.Serialization;

namespace Disqord.Serialization.Json.Default;

internal sealed class OptionalConverter<TValue> : JsonConverter
{
    private readonly JsonConverter? _valueConverter;

    public OptionalConverter(JsonConverter? valueConverter)
    {
        _valueConverter = valueConverter;
    }

    public override bool CanConvert(Type objectType)
    {
        return true;
    }

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        TValue? value;
        if (_valueConverter != null && _valueConverter.CanRead)
        {
            value = (TValue?) _valueConverter.ReadJson(reader, typeof(TValue), existingValue, serializer);
        }
        else
        {
            value = serializer.Deserialize<TValue>(reader);
        }

        return new Optional<TValue?>(value);
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        var optionalValue = ((IOptional?) value)!.Value;
        if (optionalValue == null)
        {
            writer.WriteNull();
        }
        else
        {
            if (_valueConverter != null && _valueConverter.CanWrite)
            {
                _valueConverter.WriteJson(writer, optionalValue, serializer);
            }
            else
            {
                serializer.Serialize(writer, optionalValue);
            }
        }
    }
}
