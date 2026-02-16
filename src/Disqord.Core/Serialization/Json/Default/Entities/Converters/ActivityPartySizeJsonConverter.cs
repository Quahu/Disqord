using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using Disqord.Serialization.Json;
using Newtonsoft.Json;
using Qommon;

namespace Disqord.Serialization.Json.Default;

internal sealed class ActivityPartySizeJsonConverter : JsonConverter<Optional<int[]>>
{
    public override Optional<int[]> ReadJson(JsonReader reader, Type objectType, Optional<int[]> existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
            return default;

        if (reader.TokenType != JsonToken.StartArray)
            return default;

        var result = new List<int>();
        while (reader.Read())
        {
            if (reader.TokenType == JsonToken.EndArray)
                break;

            if (reader.TokenType == JsonToken.Integer)
            {
                var value = reader.Value;
                if (value is int intValue)
                {
                    result.Add(intValue);
                }
                else if (value is long longValue)
                {
                    if (longValue > int.MaxValue)
                        result.Add(int.MaxValue);
                    else if (longValue < int.MinValue)
                        result.Add(int.MinValue);
                    else
                        result.Add((int) longValue);
                }
                else if (value is BigInteger bigIntegerValue)
                {
                    if (bigIntegerValue > int.MaxValue)
                        result.Add(int.MaxValue);
                    else if (bigIntegerValue < int.MinValue)
                        result.Add(int.MinValue);
                    else
                        result.Add((int) bigIntegerValue);
                }
                else
                {
                    result.Add(Convert.ToInt32(value));
                }
            }
            else if (reader.TokenType == JsonToken.Float)
            {
                var value = Convert.ToDouble(reader.Value);
                if (value > int.MaxValue)
                    result.Add(int.MaxValue);
                else if (value < int.MinValue)
                    result.Add(int.MinValue);
                else
                    result.Add((int) value);
            }
            else if (reader.TokenType == JsonToken.String)
            {
                var stringValue = (string) reader.Value!;
                if (long.TryParse(stringValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out var longValue))
                {
                    if (longValue > int.MaxValue)
                        result.Add(int.MaxValue);
                    else if (longValue < int.MinValue)
                        result.Add(int.MinValue);
                    else
                        result.Add((int) longValue);
                }
                else if (double.TryParse(stringValue, NumberStyles.Float, CultureInfo.InvariantCulture, out var doubleValue))
                {
                    if (doubleValue > int.MaxValue)
                        result.Add(int.MaxValue);
                    else if (doubleValue < int.MinValue)
                        result.Add(int.MinValue);
                    else
                        result.Add((int) doubleValue);
                }
                else
                {
                    // Fallback or just ignore?
                    // We should probably just ignore invalid strings or default to 0
                    result.Add(0);
                }
            }
            else
            {
                try
                {
                    var token = Newtonsoft.Json.Linq.JToken.Load(reader);
                    if (token.Type == Newtonsoft.Json.Linq.JTokenType.Integer)
                    {
                        var bigIntegerValue = token.ToObject<BigInteger>();
                        if (bigIntegerValue > int.MaxValue)
                            result.Add(int.MaxValue);
                        else if (bigIntegerValue < int.MinValue)
                            result.Add(int.MinValue);
                        else
                            result.Add((int) bigIntegerValue);
                    }
                    else
                    {
                        // Fallback
                        result.Add(0);
                    }
                }
                catch
                {
                    result.Add(0);
                }
            }
        }

        return new Optional<int[]>(result.ToArray());
    }

    public override void WriteJson(JsonWriter writer, Optional<int[]> value, JsonSerializer serializer)
    {
        if (!value.HasValue)
        {
            writer.WriteNull();
            return;
        }

        serializer.Serialize(writer, value.Value);
    }
}
