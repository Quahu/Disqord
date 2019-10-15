using Disqord.Serialization.Json;

namespace Disqord.Rest
{
    internal sealed class CreateGuildRoleContent : JsonRequestContent
    {
        [JsonProperty("name")]
        public Optional<string> Name { get; set; }

        [JsonProperty("permissions")]
        public Optional<ulong> Permissions { get; set; }

        [JsonProperty("color")]
        public Optional<int> Color { get; set; }

        [JsonProperty("hoist")]
        public Optional<bool> Hoist { get; set; }

        [JsonProperty("mentionable")]
        public Optional<bool> Mentionable { get; set; }
    }
}
