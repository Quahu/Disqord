using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models
{
    public class EmbedFooterJsonModel : JsonModel
    {
        [JsonProperty("text")]
        public string Text;

        [JsonProperty("icon_url")]
        public Optional<string> IconUrl;

        [JsonProperty("proxy_icon_url")]
        public Optional<string> ProxyIconUrl;
    }
}