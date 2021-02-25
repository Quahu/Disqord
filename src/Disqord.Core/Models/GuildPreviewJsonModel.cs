using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class GuildPreviewJsonModel : JsonModel
    {
        [JsonProperty("id")]
        public Snowflake Id;

        [JsonProperty("name")]
        public string Name = default!;

        [JsonProperty("icon")]
        public string Icon = default!;

        [JsonProperty("splash")]
        public string Splash = default!;

        [JsonProperty("discovery_splash")]
        public Optional<string> DiscoverySplash;

        [JsonProperty("emojis")]
        public EmojiJsonModel[] Emojis = default!;

        [JsonProperty("features")]
        public string[] Features = default!;

        [JsonProperty("approximate_member_count")]
        public int ApproximateMemberCount;

        [JsonProperty("approximate_presence_count")]
        public int ApproximatePresenceCount;

        [JsonProperty("description")]
        public string Description = default!;
    }
}
