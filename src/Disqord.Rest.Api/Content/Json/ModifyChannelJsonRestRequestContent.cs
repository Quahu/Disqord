﻿using Disqord.Models;
using Disqord.Serialization.Json;

namespace Disqord.Rest.Api
{
    public class ModifyChannelJsonRestRequestContent : JsonModelRestRequestContent
    {
        [JsonProperty("name")]
        public Optional<string> Name;

        [JsonProperty("type")]
        public Optional<ChannelType> Type;

        [JsonProperty("position")]
        public Optional<int> Position;

        [JsonProperty("topic")]
        public Optional<string> Topic;

        [JsonProperty("nsfw")]
        public Optional<bool> Nsfw;

        [JsonProperty("rate_limit_per_user")]
        public Optional<int> RateLimitPerUser;

        [JsonProperty("bitrate")]
        public Optional<int> Bitrate;

        [JsonProperty("user_limit")]
        public Optional<int> UserLimit;

        [JsonProperty("permission_overwrites")]
        public Optional<OverwriteJsonModel[]> PermissionOverwrites;

        [JsonProperty("parent_id")]
        public Optional<Snowflake?> ParentId;
    }
}
