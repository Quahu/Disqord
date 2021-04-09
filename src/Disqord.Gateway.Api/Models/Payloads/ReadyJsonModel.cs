using Disqord.Models;
using Disqord.Serialization.Json;

namespace Disqord.Gateway.Api.Models
{
    [JsonSkippedProperties("private_channels")]
    public class ReadyJsonModel : JsonModel
    {
        [JsonProperty("v")]
        public int V;

        [JsonProperty("user")]
        public UserJsonModel User;

        [JsonProperty("guilds")]
        public UnavailableGuildJsonModel[] Guilds;

        [JsonProperty("session_id")]
        public string SessionId;

        [JsonProperty("shard")]
        public int[] Shard;
    }
}
