using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class EmbedThumbnailJsonModel : JsonModel
{
    [JsonProperty("url")]
    public string Url = null!;

    [JsonProperty("proxy_url")]
    public Optional<string> ProxyUrl;

    [JsonProperty("height")]
    public Optional<int> Height;

    [JsonProperty("width")]
    public Optional<int> Width;
}
