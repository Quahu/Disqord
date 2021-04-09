using System.Collections.Generic;
using Disqord.Models;
using Disqord.Serialization.Json;

namespace Disqord.Gateway.Api.Models
{
    public class PresenceJsonModel : JsonModel
    {
        [JsonProperty("user")]
        public UserJsonModel User;

        [JsonProperty("guild_id")]
        public Snowflake GuildId;

        [JsonProperty("status")]
        public UserStatus Status;

        [JsonProperty("activities")]
        public ActivityJsonModel[] Activities;

        [JsonProperty("client_status")]
        public Dictionary<UserClient, UserStatus> ClientStatus;
    }
}
