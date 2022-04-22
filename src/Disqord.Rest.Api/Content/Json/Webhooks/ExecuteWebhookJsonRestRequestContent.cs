using Disqord.Models;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Rest.Api
{
    public class ExecuteWebhookJsonRestRequestContent : JsonModelRestRequestContent
    {
        [JsonProperty("content")]
        public Optional<string> Content;

        [JsonProperty("username")]
        public Optional<string> Username;

        [JsonProperty("avatar_url")]
        public Optional<string> AvatarUrl;

        [JsonProperty("tts")]
        public Optional<bool> Tts;

        [JsonProperty("embeds")]
        public Optional<EmbedJsonModel[]> Embeds;

        [JsonProperty("allowed_mentions")]
        public Optional<AllowedMentionsJsonModel> AllowedMentions;

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
