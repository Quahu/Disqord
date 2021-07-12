﻿using Disqord.Serialization.Json;

namespace Disqord.Rest.Api
{
    public class ModifyGuildWidgetSettingsJsonRestRequestContent : JsonModelRestRequestContent
    {
        [JsonProperty("enabled")] 
        public Optional<bool> Enabled;

        [JsonProperty("channel_id")]
        public Optional<Snowflake?> ChannelId;
    }
}