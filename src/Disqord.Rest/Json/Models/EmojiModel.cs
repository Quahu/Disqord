using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal sealed class EmojiModel
    {
        [JsonProperty("id")]
        public ulong? Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("roles")]
        public ulong[] Roles { get; set; }

        [JsonProperty("user")]
        public UserModel User { get; set; }

        [JsonProperty("require_colons")]
        public bool RequireColons { get; set; }

        [JsonProperty("managed")]
        public bool Managed { get; set; }

        [JsonProperty("animated")]
        public bool Animated { get; set; }

        [JsonProperty("available")]
        public bool Available { get; set; }
    }
}
