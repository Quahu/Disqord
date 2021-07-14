using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class ApplicationCommandPermissionsJsonModel : JsonModel
    {
        [JsonProperty("id")]
        public Snowflake Id;

        [JsonProperty("type")]
        public ApplicationCommandPermissionType Type;

        [JsonProperty("permission")]
        public bool Permission;
    }
}
