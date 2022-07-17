using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class EmbedVideoJsonModel : JsonModel
{
    [JsonProperty("url")]
    public Optional<string> Url;

    [JsonProperty("height")]
    public Optional<int> Height;

    [JsonProperty("width")]
    public Optional<int> Width;
}