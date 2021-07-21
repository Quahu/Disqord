using Disqord.Serialization.Json;

namespace Disqord.Rest.Api
{
    public class ModifyGuildTemplateJsonRestRequestContent : JsonModelRestRequestContent
    {
        [JsonProperty("name")]
        public Optional<string> Name;

        [JsonProperty("description")]
        public Optional<string> Description;
    }
}
