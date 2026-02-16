using System;
using System.Globalization;
using System.Numerics;
using Newtonsoft.Json;

namespace Disqord.Serialization.Json.Default;

internal sealed class ClampingInt32Converter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(int) || objectType == typeof(int?);
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
        {
            if (objectType == typeof(int?))
                return null;

            return 0;
        }

        if (reader.TokenType == JsonToken.Integer)
        {
            var value = reader.Value;
            if (value is int intValue)
                return intValue;

            if (value is long longValue)
            {
                if (longValue > int.MaxValue)
                    return int.MaxValue;

                if (longValue < int.MinValue)
                    return int.MinValue;

                return (int) longValue;
            }

            if (value is BigInteger bigIntegerValue)
            {
                if (bigIntegerValue > int.MaxValue)
                    return int.MaxValue;

                if (bigIntegerValue < int.MinValue)
                    return int.MinValue;

                return (int) bigIntegerValue;
            }

            return Convert.ToInt32(value);
        }

        if (reader.TokenType == JsonToken.Float)
        {
            var value = Convert.ToDouble(reader.Value);
            if (value > int.MaxValue)
                return int.MaxValue;

            if (value < int.MinValue)
                return int.MinValue;

            return (int) value;
        }

        if (reader.TokenType == JsonToken.String)
        {
            var stringValue = (string) reader.Value!;
            if (long.TryParse(stringValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out var longValue))
            {
                if (longValue > int.MaxValue)
                    return int.MaxValue;

                if (longValue < int.MinValue)
                    return int.MinValue;

                return (int) longValue;
            }

            if (double.TryParse(stringValue, NumberStyles.Float, CultureInfo.InvariantCulture, out var doubleValue))
            {
                if (doubleValue > int.MaxValue)
                    return int.MaxValue;

                if (doubleValue < int.MinValue)
                    return int.MinValue;

                return (int) doubleValue;
            }
        }

        try
        {
            var token = Newtonsoft.Json.Linq.JToken.Load(reader);
            if (token.Type == Newtonsoft.Json.Linq.JTokenType.Integer)
            {
                var bigIntegerValue = token.ToObject<BigInteger>();
                if (bigIntegerValue > int.MaxValue)
                    return int.MaxValue;

                if (bigIntegerValue < int.MinValue)
                    return int.MinValue;

                return (int) bigIntegerValue;
            }

            return token.ToObject(objectType);
        }
        catch
        {
            return 0;
        }
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        writer.WriteValue(value);
    }
}
