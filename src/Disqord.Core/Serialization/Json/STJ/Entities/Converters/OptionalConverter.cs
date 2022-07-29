using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Qommon;
using Qommon.Serialization;

namespace Disqord.Serialization.Json.STJ.Converters;

internal class OptionalConverter : JsonConverter<IOptional>
{
    public override IOptional? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return (IOptional?)typeToConvert.GetConstructors()[0].Invoke(new[] { JsonSerializer.Deserialize(ref reader, typeToConvert.GenericTypeArguments[0], options) });
    }

    public override void Write(Utf8JsonWriter writer, IOptional value, JsonSerializerOptions options)
    {
        var optionalValue = ((IOptional?)value)!.Value;
        if (optionalValue == null)
        {
            writer.WriteNullValue();
        }

        JsonSerializer.Serialize(optionalValue, options);
    }
}