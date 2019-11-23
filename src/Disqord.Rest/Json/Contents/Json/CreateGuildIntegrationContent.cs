using Disqord.Serialization.Json;

namespace Disqord.Rest
{
    internal sealed class CreateGuildIntegrationContent : JsonRequestContent
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
