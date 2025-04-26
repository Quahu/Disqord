using Disqord.Serialization.Json;

namespace Disqord.Models;

public class ResolvedUnfurledMediaItemJsonModel : UnfurledMediaItemJsonModel
{
    [JsonProperty("proxy_url")]
    public string? ProxyUrl;

    [JsonProperty("width")]
    public int? Width;

    [JsonProperty("height")]
    public int? Height;

    [JsonProperty("content_type")]
    public string? ContentType;

    [JsonProperty("loading_state")]
    public UnfurledMediaItemLoadingState? LoadingState;
}
