using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Disqord.Serialization.Json.STJ.Converters;

internal class SnowflakeConverter : JsonConverter<Snowflake?>
{
    public override bool HandleNull => true;
    public override Snowflake? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TryGetUInt64(out var value))
        {
            return new Snowflake(value);
        }

        return null;
    }

    public override void Write(Utf8JsonWriter writer, Snowflake? value, JsonSerializerOptions options)
    {
        if (value.HasValue)
        {
            writer.WriteNumberValue(value.Value.RawValue);
        }
        writer.WriteNullValue();
    }
}