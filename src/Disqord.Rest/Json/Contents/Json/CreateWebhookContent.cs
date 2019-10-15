using Disqord.Serialization.Json;

namespace Disqord.Rest
{
    internal sealed class CreateWebhookContent : JsonRequestContent
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("avatar", NullValueHandling = NullValueHandling.Ignore)]
        public LocalAttachment Avatar { get; set; }
    }
}
