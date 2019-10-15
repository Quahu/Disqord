using Disqord.Serialization.Json;

namespace Disqord.Models.Dispatches
{
    internal sealed class IdentifyModel
    {
        [JsonProperty("token", NullValueHandling = NullValueHandling.Ignore)]
        public string Token { get; set; }

        [JsonProperty("properties", NullValueHandling = NullValueHandling.Ignore)]
        public ConnectionProperties Properties { get; set; } = new ConnectionProperties();

        [JsonProperty("compress", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Compress { get; set; }

        [JsonProperty("large_threshold", NullValueHandling = NullValueHandling.Ignore)]
        public int? LargeThreshold { get; set; }

        [JsonProperty("shards", NullValueHandling = NullValueHandling.Ignore)]
        public int[] Shards { get; set; }

        [JsonProperty("presence", NullValueHandling = NullValueHandling.Ignore)]
        public UpdateStatusModel Presence { get; set; }

        [JsonProperty("guild_subscriptions")]
        public bool GuildSubscriptions { get; set; }

        public sealed class ConnectionProperties
        {
            [JsonProperty("$os", NullValueHandling = NullValueHandling.Ignore)]
            public string Os { get; set; }

            [JsonProperty("$browser", NullValueHandling = NullValueHandling.Ignore)]
            public string Browser { get; set; }

            [JsonProperty("$device", NullValueHandling = NullValueHandling.Ignore)]
            public string Device { get; set; }
        }
    }
}
