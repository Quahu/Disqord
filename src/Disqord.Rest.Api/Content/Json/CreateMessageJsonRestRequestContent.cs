﻿using Disqord.Models;
using Disqord.Serialization.Json;

namespace Disqord.Rest.Api
{
    public class CreateMessageJsonRestRequestContent : JsonModelRestRequestContent
    {
        [JsonProperty("content")]
        public Optional<string> Content;

        [JsonProperty("nonce")]
        public Optional<string> Nonce;

        [JsonProperty("tts")]
        public Optional<bool> Tts;

        [JsonProperty("embeds")]
        public Optional<EmbedJsonModel[]> Embeds;

        [JsonProperty("allowed_mentions")]
        public Optional<AllowedMentionsJsonModel> AllowedMentions;

        [JsonProperty("message_reference")]
        public Optional<MessageReferenceJsonModel> MessageReference;

        [JsonProperty("components")]
        public Optional<ComponentJsonModel[]> Components;

        [JsonProperty("sticker_ids")]
        public Optional<Snowflake[]> StickerIds;
    }
}
