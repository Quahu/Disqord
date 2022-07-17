using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Gateway.Api.Models;

public class ActivityAssetsJsonModel : JsonModel
{
    [JsonProperty("large_image")]
    public Optional<string> LargeImage;

    [JsonProperty("large_text")]
    public Optional<string> LargeText;

    [JsonProperty("small_image")]
    public Optional<string> SmallImage;

    [JsonProperty("small_text")]
    public Optional<string> SmallText;
}