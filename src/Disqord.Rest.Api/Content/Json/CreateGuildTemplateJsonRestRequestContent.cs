using Disqord.Serialization.Json;

namespace Disqord.Rest.Api
{
    public class CreateGuildTemplateJsonRestRequestContent : JsonModelRestRequestContent
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("description")]
        public Optional<string> Description;

        public CreateGuildTemplateJsonRestRequestContent(string name)
        {
            Name = name;
        }
    }
}
