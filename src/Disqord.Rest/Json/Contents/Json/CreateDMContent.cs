using Disqord.Serialization.Json;

namespace Disqord.Rest
{
    internal sealed class CreateDMContent : JsonRequestContent
    {
        [JsonProperty("recipient_id")]
        public ulong RecipientId { get; set; }
    }
}
