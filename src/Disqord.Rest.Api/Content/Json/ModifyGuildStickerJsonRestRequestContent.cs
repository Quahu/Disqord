using Disqord.Serialization.Json;

namespace Disqord.Rest.Api
{
    public class ModifyGuildStickerJsonRestRequestContent : JsonModelRestRequestContent
    {
        [JsonProperty("name")]
        public Optional<string> Name;

        [JsonProperty("description")]
        public Optional<string> Description;

        [JsonProperty("tags")]
        public Optional<string> Tags;
    }
}