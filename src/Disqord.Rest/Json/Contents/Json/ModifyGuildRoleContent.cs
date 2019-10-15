using Disqord.Serialization.Json;

namespace Disqord.Rest
{
    internal sealed class ModifyGuildRoleContent : JsonRequestContent
    {
        [JsonProperty("name")]
        public Optional<string> Name { internal get; set; }

        [JsonProperty("permissions")]
        public Optional<ulong> Permissions { internal get; set; }

        [JsonProperty("color")]
        public Optional<int> Color { internal get; set; }

        [JsonProperty("hoist")]
        public Optional<bool> Hoist { internal get; set; }

        [JsonProperty("mentionable")]
        public Optional<bool> Mentionable { internal get; set; }
    }
}
