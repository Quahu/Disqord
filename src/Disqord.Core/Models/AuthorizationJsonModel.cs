using System;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models
{
    public class AuthorizationJsonModel : JsonModel
    {
        [JsonProperty("application")]
        public ApplicationJsonModel Application;

        [JsonProperty("scopes")]
        public string[] Scopes;

        [JsonProperty("expires")]
        public DateTimeOffset Expires;

        [JsonProperty("user")]
        public Optional<UserJsonModel> User;
    }
}
