using Disqord.Serialization.Json;

namespace Disqord.Models.Dispatches
{
    internal sealed class IdentifyModel
    {
        [JsonProperty("token", NullValueHandling.Ignore)]
        public string Token { get; set; }

        [JsonProperty("properties", NullValueHandling.Ignore)]
        public ConnectionProperties Properties { get; set; } = new ConnectionProperties();

        [JsonProperty("compress", NullValueHandling.Ignore)]
        public bool? Compress { get; set; }

        [JsonProperty("large_threshold", NullValueHandling.Ignore)]
        public int? LargeThreshold { get; set; }

        [JsonProperty("shard", NullValueHandling.Ignore)]
        public int[] Shard { get; set; }

        [JsonProperty("presence", NullValueHandling.Ignore)]
        public UpdateStatusModel Presence { get; set; }

        [JsonProperty("guild_subscriptions")]
        public bool GuildSubscriptions { get; set; } = true;

        public sealed class ConnectionProperties
        {
            [JsonProperty("$os", NullValueHandling.Ignore)]
            public string Os { get; set; }

            [JsonProperty("$browser", NullValueHandling.Ignore)]
            public string Browser { get; set; }

            [JsonProperty("$device", NullValueHandling.Ignore)]
            public string Device { get; set; }
        }
    }
}
