using System;
using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class MemberJsonModel : JsonModel
    {
        [JsonProperty("user")]
        public Optional<UserJsonModel> User;

        [JsonProperty("nick")]
        public string Nick;

        [JsonProperty("roles")]
        public Snowflake[] Roles;

        [JsonProperty("joined_at")]
        public DateTimeOffset JoinedAt;

        [JsonProperty("premium_since")]
        public Optional<DateTimeOffset?> PremiumSince;

        [JsonProperty("deaf")]
        public bool Deaf;

        [JsonProperty("mute")]
        public bool Mute;

        [JsonProperty("pending")]
        public bool Pending;
    }
}
