using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class InteractionCallbackDataJsonModel : JsonModel
    {
        [JsonProperty("tts")]
        public Optional<bool> Tts;

        [JsonProperty("content")]
        public Optional<string> Content;

        [JsonProperty("embeds")]
        public Optional<EmbedJsonModel[]> Embeds;

        [JsonProperty("allowed_mentions")]
        public Optional<AllowedMentionsJsonModel> AllowedMentions;

        /// <summary>
        ///     Set to 64 to make your response ephemeral, <c>0oH4tvG70Eo</c>
        /// </summary>
        [JsonProperty("flags")]
        public Optional<InteractionResponseFlag> Flags;

        [JsonProperty("components")]
        public Optional<ComponentJsonModel[]> Components;
    }
}
