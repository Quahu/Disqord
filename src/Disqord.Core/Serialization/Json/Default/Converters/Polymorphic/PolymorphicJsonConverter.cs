using System.Text.Json;
using System.Text.Json.Serialization;

namespace Disqord.Serialization.Json.Default;

internal interface IPolymorphicJsonConverter
{
    void SetOptionsWithoutSelf(JsonSerializerOptions options);
}

internal abstract class PolymorphicJsonConverter<T> : JsonConverter<T>, IPolymorphicJsonConverter
{
    protected JsonSerializerOptions OptionsWithoutSelf { get; private set; } = null!;

    protected void WritePolymorphic(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        var runtimeType = value!.GetType();
        var serializeOptions = runtimeType == typeof(T) ? OptionsWithoutSelf : options;
        JsonSerializer.Serialize(writer, value, runtimeType, serializeOptions);
    }

    void IPolymorphicJsonConverter.SetOptionsWithoutSelf(JsonSerializerOptions options)
    {
        OptionsWithoutSelf = options;
    }
}
