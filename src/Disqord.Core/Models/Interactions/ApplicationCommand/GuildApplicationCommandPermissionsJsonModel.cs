using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class GuildApplicationCommandPermissionsJsonModel : JsonModel
    {
        [JsonProperty("id")]
        public Snowflake Id;
        
        [JsonProperty("application_id")]
        public Snowflake ApplicationId;
        
        [JsonProperty("guild_id")]
        public Snowflake GuildId;
        
        [JsonProperty("permissions")]
        public ApplicationCommandPermissionsJsonModel[] Permissions;
    }
}
