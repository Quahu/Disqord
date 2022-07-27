using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Gateway.Api.Models;

public class IdentifyJsonModel : JsonModel
{
    [JsonProperty("token")]
    public string Token = null!;

    [JsonProperty("properties")]
    public PropertiesJsonModel Properties = null!;

    [JsonProperty("compress")]
    public Optional<bool> Compress;

    [JsonProperty("large_threshold")]
    public Optional<int> LargeThreshold;

    [JsonProperty("shard")]
    public Optional<int[]> Shard;

    [JsonProperty("presence")]
    public Optional<UpdatePresenceJsonModel> Presence;

    [JsonProperty("guild_subscriptions")]
    public Optional<bool> GuildSubscriptions;

    [JsonProperty("intents")]
    public GatewayIntents Intents;

    public class PropertiesJsonModel : JsonModel
    {
        [JsonProperty("os")]
        public string Os = null!;

        [JsonProperty("browser")]
        public string Browser = null!;

        [JsonProperty("device")]
        public string Device = null!;
    }
}
