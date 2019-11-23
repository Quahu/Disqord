using System.Collections.Generic;
using Disqord.Models;
using Disqord.Serialization.Json;

namespace Disqord.Rest
{
    internal sealed class ExecuteWebhookContent : JsonRequestContent
    {
        [JsonProperty("username", NullValueHandling.Ignore)]
        public string Username { get; set; }

        [JsonProperty("avatar_url", NullValueHandling.Ignore)]
        public string AvatarUrl { get; set; }

        [JsonProperty("content", NullValueHandling.Ignore)]
        public string Content { get; set; }

        [JsonProperty("nonce", NullValueHandling.Ignore)]
        public ulong? Nonce { get; set; }

        [JsonProperty("tts", NullValueHandling.Ignore)]
        public bool TTS { get; set; }

        [JsonProperty("embeds", NullValueHandling.Ignore)]
        public IReadOnlyList<EmbedModel> Embeds { get; set; }
    }
}
