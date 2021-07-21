﻿using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class StageInstanceJsonModel : JsonModel
    {
        [JsonProperty("id")]
        public Snowflake Id;

        [JsonProperty("guild_id")]
        public Snowflake GuildId;

        [JsonProperty("channel_id")]
        public Snowflake ChannelId;

        [JsonProperty("topic")]
        public string Topic;

        [JsonProperty("privacy_level")]
        public StagePrivacyLevel PrivacyLevel;

        [JsonProperty("discoverable_disabled")]
        public bool DiscoverableDisabled;
    }
}
