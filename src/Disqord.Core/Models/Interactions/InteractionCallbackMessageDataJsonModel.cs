using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models
{
    public class InteractionCallbackMessageDataJsonModel : JsonModel
    {
        [JsonProperty("tts")]
        public Optional<bool> Tts;

        [JsonProperty("content")]
        public Optional<string> Content;

        [JsonProperty("embeds")]
        public Optional<EmbedJsonModel[]> Embeds;

        [JsonProperty("allowed_mentions")]
        public Optional<AllowedMentionsJsonModel> AllowedMentions;

        [JsonProperty("flags")]
        public Optional<MessageFlag> Flags;

        [JsonProperty("components")]
        public Optional<ComponentJsonModel[]> Components;

        protected override void OnValidate()
        {
            OptionalGuard.CheckValue(Components, components =>
            {
                for (var i = 0; i < components.Length; i++)
                    components[i].Validate();
            });
        }
    }
}
