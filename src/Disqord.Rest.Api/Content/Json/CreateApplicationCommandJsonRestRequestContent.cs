using Disqord.Models;
using Disqord.Serialization.Json;

namespace Disqord.Rest.Api
{
    public class CreateApplicationCommandJsonRestRequestContent : JsonModelRestRequestContent
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("description")]
        public string Description;

        [JsonProperty("options")]
        public Optional<ApplicationCommandOptionJsonModel[]> Options;

        [JsonProperty("default_permission")]
        public Optional<bool> DefaultPermission;

        [JsonProperty("type")]
        public Optional<ApplicationCommandType> Type;
    }
}
