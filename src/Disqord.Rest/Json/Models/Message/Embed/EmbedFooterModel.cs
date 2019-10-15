using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal sealed class EmbedFooterModel
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("icon_url")]
        public string IconUrl { get; set; }

        [JsonProperty("proxy_icon_url")]
        public string ProxyIconUrl { get; set; }
    }
}
