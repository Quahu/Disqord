using System.Text.Json;
using System.Text.Json.Serialization;

namespace Disqord.Serialization.Json.Default;

internal abstract class PolymorphicJsonConverter<T> : JsonConverter<T>, IPolymorphicJsonConverter
{
    public JsonSerializerOptions? OptionsWithoutSelf { get; private set; }
    public JsonSerializerOptions? OptionsWithPreserve { get; private set; }

    protected JsonSerializerOptions GetPolymorphicOptions(object value, JsonSerializerOptions options)
    {
        return CanConvert(value.GetType())
            ? OptionsWithoutSelf ?? options
            : options;
    }

    public void SetOptionsWithoutSelf(JsonSerializerOptions options)
    {
        OptionsWithoutSelf = options;
    }
    
    public void SetOptionsWithPreserve(JsonSerializerOptions options)
    {
        OptionsWithPreserve = options;
    }
}
