using System.Collections.Generic;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models
{
    public class AllowedMentionsJsonModel : JsonModel
    {
        [JsonProperty("parse")]
        public IList<string> Parse;

        [JsonProperty("users", NullValueHandling.Ignore)]
        public Snowflake[] Users;

        [JsonProperty("roles", NullValueHandling.Ignore)]
        public Snowflake[] Roles;

        [JsonProperty("replied_user")]
        public Optional<bool> RepliedUser;
    }
}
