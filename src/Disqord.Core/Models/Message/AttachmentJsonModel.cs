using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class AttachmentJsonModel : JsonModel
    {
        [JsonProperty("id")]
        public Snowflake Id;

        [JsonProperty("filename")]
        public string FileName;

        [JsonProperty("content_type")]
        public Optional<string> ContentType;

        [JsonProperty("size")]
        public int Size;

        [JsonProperty("url")]
        public string Url;

        [JsonProperty("proxy_url")]
        public string ProxyUrl;

        [JsonProperty("height")]
        public Optional<int> Height;

        [JsonProperty("width")]
        public Optional<int> Width;
    }
}
