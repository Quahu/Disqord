using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal sealed class EmbedAuthorModel
    {
        [JsonProperty("name", NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("url", NullValueHandling.Ignore)]
        public string Url { get; set; }

        [JsonProperty("icon_url", NullValueHandling.Ignore)]
        public string IconUrl { get; set; }

        [JsonProperty("proxy_icon_url", NullValueHandling.Ignore)]
        public string ProxyIconUrl { get; set; }
    }
}
