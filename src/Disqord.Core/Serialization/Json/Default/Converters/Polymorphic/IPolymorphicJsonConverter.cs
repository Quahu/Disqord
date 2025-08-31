using System.Text.Json;

namespace Disqord.Serialization.Json.Default;

internal interface IPolymorphicJsonConverter
{
    void SetOptionsWithoutSelf(JsonSerializerOptions options);

    void SetOptionsWithPreserve(JsonSerializerOptions options);
}
