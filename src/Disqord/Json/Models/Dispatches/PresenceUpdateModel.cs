using System.Collections.Generic;
using Disqord.Serialization.Json;

namespace Disqord.Models.Dispatches
{
    internal sealed class PresenceUpdateModel
    {
        [JsonProperty("user", NullValueHandling = NullValueHandling.Ignore)]
        public UserModel User { get; set; }

        [JsonProperty("roles", NullValueHandling = NullValueHandling.Ignore)]
        public ulong[] Roles { get; set; }

        [JsonProperty("nick", NullValueHandling = NullValueHandling.Ignore)]
        public string Nick { get; set; }

        [JsonProperty("game", NullValueHandling = NullValueHandling.Ignore)]
        public ActivityModel Activity { get; set; }

        [JsonProperty("guild_id", NullValueHandling = NullValueHandling.Ignore)]
        public ulong? GuildId { get; set; }

        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public UserStatus Status { get; set; }

        [JsonProperty("client_status", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, UserStatus> ClientStatus { get; set; }

        [JsonProperty("activities", NullValueHandling = NullValueHandling.Ignore)]
        public ActivityModel[] Activities { get; set; }
    }
}
