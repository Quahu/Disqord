using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models
{
    public class GuildPreviewJsonModel : JsonModel
    {
        [JsonProperty("id")]
        public Snowflake Id;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("icon")]
        public string Icon;

        [JsonProperty("splash")]
        public string Splash;

        [JsonProperty("discovery_splash")]
        public Optional<string> DiscoverySplash;

        [JsonProperty("emojis")]
        public EmojiJsonModel[] Emojis;

        [JsonProperty("features")]
        public string[] Features;

        [JsonProperty("approximate_member_count")]
        public int ApproximateMemberCount;

        [JsonProperty("approximate_presence_count")]
        public int ApproximatePresenceCount;

        [JsonProperty("description")]
        public string Description;
    }
}
