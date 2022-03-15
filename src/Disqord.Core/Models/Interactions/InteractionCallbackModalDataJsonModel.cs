using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models
{
    public class InteractionCallbackModalDataJsonModel : JsonModel
    {
        [JsonProperty("custom_id")]
        public string CustomId;

        [JsonProperty("title")]
        public string Title;

        [JsonProperty("components")]
        public ComponentJsonModel[] Components;

        protected override void OnValidate()
        {
            Guard.IsNotNull(CustomId);
            Guard.IsLessThanOrEqualTo(CustomId.Length, Discord.Limits.Interactions.Modals.MaxCustomIdLength);

            Guard.IsNotEmpty(Title);
            Guard.IsLessThanOrEqualTo(CustomId.Length, Discord.Limits.Interactions.Modals.MaxTitleLength);

            Guard.IsNotEmpty(Components);
            foreach (var component in Components)
            {
                Guard.IsNotNull(component);
                component.Validate();
            }
        }
    }
}
