using Disqord.Models.Slash;
using Disqord.Serialization.Json;

namespace Disqord.Rest.Api
{
    public class ModifySlashCommandJsonRestRequestContent : JsonModelRestRequestContent
    {
        [JsonProperty("name")]
        public Optional<string> Name;

        [JsonProperty("description")]
        public Optional<string> Description;

        [JsonProperty("options")]
        public Optional<SlashCommandOptionJsonModel[]> Options;
    }
}
