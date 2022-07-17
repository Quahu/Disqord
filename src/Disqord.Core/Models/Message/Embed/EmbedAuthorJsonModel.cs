using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class EmbedAuthorJsonModel : JsonModel
{
    [JsonProperty("name")]
    public string Name = null!;

    [JsonProperty("url")]
    public Optional<string> Url;

    [JsonProperty("icon_url")]
    public Optional<string> IconUrl;

    [JsonProperty("proxy_icon_url")]
    public Optional<string> ProxyIconUrl;
}
