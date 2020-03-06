using Disqord.Models;
using Disqord.Serialization.Json;

namespace Disqord.Rest
{
    internal sealed class CreateMessageContent : JsonRequestContent
    {
        [JsonProperty("content", NullValueHandling.Ignore)]
        public string Content { get; set; }

        [JsonProperty("nonce", NullValueHandling.Ignore)]
        public ulong? Nonce { get; set; }

        [JsonProperty("tts")]
        public bool Tts { get; set; }

        [JsonProperty("embed", NullValueHandling.Ignore)]
        public EmbedModel Embed { get; set; }

        [JsonProperty("allowed_mentions", NullValueHandling.Ignore)]
        public AllowedMentionsModel AllowedMentions { get; set; }
    }
}
