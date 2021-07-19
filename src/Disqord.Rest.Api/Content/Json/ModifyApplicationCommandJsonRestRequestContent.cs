using Disqord.Models;
using Disqord.Serialization.Json;

namespace Disqord.Rest.Api
{
    public class ModifyApplicationCommandJsonRestRequestContent : JsonModelRestRequestContent
    {
        [JsonProperty("name")]
        public Optional<string> Name;

        [JsonProperty("description")]
        public Optional<string> Description;

        [JsonProperty("options")]
        public Optional<ApplicationCommandOptionJsonModel[]> Options;

        [JsonProperty("default_permission")]
        public Optional<bool> DefaultPermission;
    }
}
