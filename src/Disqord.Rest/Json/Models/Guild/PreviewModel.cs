using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal class PreviewModel
    {
        [JsonProperty("id")]
        public ulong Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("splash")]
        public string Splash { get; set; }

        [JsonProperty("discovery_splash")]
        public string DiscoverySplash { get; set; }

        [JsonProperty("emojis")]
        public EmojiModel[] Emojis { get; set; }

        [JsonProperty("features")]
        public string[] Features { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("approximate_presence_count")]
        public int ApproximatePresenceCount { get; set; }

        [JsonProperty("approximate_member_count")]
        public int ApproximateMemberCount { get; set; }
    }
}
