using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal sealed class ActivityModel
    {
        [JsonProperty("name", NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("type")]
        public ActivityType Type { get; set; }

        [JsonProperty("created_at", NullValueHandling.Ignore)]
        public long? CreatedAt { get; set; }

        [JsonProperty("url", NullValueHandling.Ignore)]
        public string Url { get; set; }

        [JsonProperty("timestamps", NullValueHandling.Ignore)]
        public TimestampsModel Timestamps { get; set; }

        [JsonProperty("application_id", NullValueHandling.Ignore)]
        public ulong? ApplicationId { get; set; }

        [JsonProperty("details", NullValueHandling.Ignore)]
        public string Details { get; set; }

        [JsonProperty("state", NullValueHandling.Ignore)]
        public string State { get; set; }

        [JsonProperty("party", NullValueHandling.Ignore)]
        public PartyModel Party { get; set; }

        [JsonProperty("Assets", NullValueHandling.Ignore)]
        public AssetsModel Assets { get; set; }

        [JsonProperty("Secrets", NullValueHandling.Ignore)]
        public SecretsModel Secrets { get; set; }

        [JsonProperty("instance", NullValueHandling.Ignore)]
        public bool? Instance { get; set; }

        [JsonProperty("sync_id", NullValueHandling.Ignore)]
        public string SyncId { get; set; }

        [JsonProperty("session_id", NullValueHandling.Ignore)]
        public string SessionId { get; set; }

        [JsonProperty("flags", NullValueHandling.Ignore)]
        public ActivityFlags? Flags { get; set; }

        [JsonProperty("emoji", NullValueHandling.Ignore)]
        public EmojiModel Emoji { get; set; }
    }
}
