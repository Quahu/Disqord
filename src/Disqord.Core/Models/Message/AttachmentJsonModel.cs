using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class AttachmentJsonModel : JsonModel
    {
        [JsonProperty("id")]
        public Snowflake Id;

        [JsonProperty("filename")]
        public string Filename;

        [JsonProperty("size")]
        public int Size;

        [JsonProperty("url")]
        public string Url;

        [JsonProperty("proxy_url")]
        public string ProxyUrl;

        [JsonProperty("height")]
        public int? Height;

        [JsonProperty("width")]
        public int? Width;
    }
}