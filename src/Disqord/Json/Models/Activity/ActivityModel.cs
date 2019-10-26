using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal sealed class ActivityModel
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public ActivityType Type { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }

        [JsonProperty("timestamps", NullValueHandling = NullValueHandling.Ignore)]
        public TimestampsModel Timestamps { get; set; }

        [JsonProperty("application_id", NullValueHandling = NullValueHandling.Ignore)]
        public string ApplicationId { get; set; }

        [JsonProperty("details", NullValueHandling = NullValueHandling.Ignore)]
        public string Details { get; set; }

        [JsonProperty("state", NullValueHandling = NullValueHandling.Ignore)]
        public string State { get; set; }

        [JsonProperty("party", NullValueHandling = NullValueHandling.Ignore)]
        public PartyModel Party { get; set; }

        [JsonProperty("Assets", NullValueHandling = NullValueHandling.Ignore)]
        public AssetsModel Assets { get; set; }

        [JsonProperty("Secrets", NullValueHandling = NullValueHandling.Ignore)]
        public SecretsModel Secrets { get; set; }

        [JsonProperty("instance", NullValueHandling = NullValueHandling.Ignore)]
        public bool Instance { get; set; }

        [JsonProperty("sync_id", NullValueHandling = NullValueHandling.Ignore)]
        public string SyncId { get; set; }

        [JsonProperty("session_id", NullValueHandling = NullValueHandling.Ignore)]
        public string SessionId { get; set; }

        [JsonProperty("flags", NullValueHandling = NullValueHandling.Ignore)]
        public ActivityFlags? Flags { get; set; }

        [JsonProperty("emoji", NullValueHandling = NullValueHandling.Ignore)]
        public EmojiModel Emoji { get; set; }
    }
}
