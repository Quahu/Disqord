using System;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models
{
    public class ThreadMemberJsonModel : JsonModel
    {
        [JsonProperty("id")]
        public Optional<Snowflake> Id;

        [JsonProperty("user_id")]
        public Optional<Snowflake> UserId;

        [JsonProperty("join_timestamp")]
        public DateTimeOffset JoinTimestamp;

        [JsonProperty("flags")]
        public int Flags;
    }
}
