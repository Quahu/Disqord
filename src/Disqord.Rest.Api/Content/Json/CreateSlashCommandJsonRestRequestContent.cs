using Disqord.Models.Slash;
using Disqord.Serialization.Json;

namespace Disqord.Rest.Api
{
    public class CreateSlashCommandJsonRestRequestContent : JsonModelRestRequestContent
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("description")]
        public Optional<string> Description;

        [JsonProperty("options")]
        public Optional<SlashCommandOptionJsonModel[]> Options;

        public CreateSlashCommandJsonRestRequestContent(string name)
        {
            Name = name;
        }
    }
}
