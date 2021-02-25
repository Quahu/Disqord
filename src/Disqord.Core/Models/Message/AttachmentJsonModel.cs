using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class AttachmentJsonModel : JsonModel
    {
        [JsonProperty("id")]
        public Snowflake Id;

        [JsonProperty("filename")]
        public string Filename = default!;

        [JsonProperty("size")]
        public int Size;

        [JsonProperty("url")]
        public string Url = default!;

        [JsonProperty("proxy_url")]
        public string ProxyUrl = default!;

        [JsonProperty("height")]
        public int? Height;

        [JsonProperty("width")]
        public int? Width;
    }
}