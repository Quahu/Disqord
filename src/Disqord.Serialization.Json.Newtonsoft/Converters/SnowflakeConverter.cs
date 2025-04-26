using System;
using System.Globalization;
using Newtonsoft.Json;

namespace Disqord.Serialization.Json.Newtonsoft;

internal sealed class SnowflakeConverter : JsonConverter
{
    public override bool CanConvert(Type typeToConvert)
    {
        return true;
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var value = reader.Value;
        return value != null
            ? new Snowflake(Convert.ToUInt64(value, CultureInfo.InvariantCulture))
            : null;
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        writer.WriteValue(((Snowflake?) value)?.RawValue);
    }
}
