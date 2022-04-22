using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models
{
    public class RoleJsonModel : JsonModel
    {
        [JsonProperty("id")]
        public Snowflake Id;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("color")]
        public int Color;

        [JsonProperty("hoist")]
        public bool Hoist;

        [JsonProperty("icon")]
        public Optional<string> Icon;

        [JsonProperty("position")]
        public int Position;

        [JsonProperty("permissions")]
        public ulong Permissions;

        [JsonProperty("managed")]
        public bool Managed;

        [JsonProperty("mentionable")]
        public bool Mentionable;

        [JsonProperty("unicode_emoji")]
        public Optional<string> UnicodeEmoji;

        [JsonProperty("tags")]
        public Optional<RoleTagsJsonModel> Tags;
    }
}
