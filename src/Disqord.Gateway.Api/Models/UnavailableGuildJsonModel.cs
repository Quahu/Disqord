using Disqord.Serialization.Json;

namespace Disqord.Gateway.Api.Models
{
    public class UnavailableGuildJsonModel : JsonModel
    {
        [JsonProperty("id")]
        public Snowflake Id;

        [JsonProperty("unavailable")]
        public bool Unavailable;
    }
}