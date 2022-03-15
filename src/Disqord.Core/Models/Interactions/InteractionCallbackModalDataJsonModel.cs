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
            Guard.IsNotEmpty(Title);
            Guard.IsNotEmpty(Components);
        }
    }
}
