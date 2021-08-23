using System;
using Disqord.Serialization.Json;

namespace Disqord.Rest.Api
{
    public class ModifyGuildEventJsonRestRequestContent : JsonModelRestRequestContent
    {
        [JsonProperty("channel_id")]
        public Optional<Snowflake> ChannelId;

        [JsonProperty("name")]
        public Optional<string> Name;

        [JsonProperty("privacy_level")]
        public Optional<StagePrivacyLevel> PrivacyLevel;

        [JsonProperty("scheduled_start_time")]
        public Optional<DateTimeOffset> ScheduledStartTime;

        [JsonProperty("description")]
        public Optional<string> Description;

        [JsonProperty("entity_type")]
        public Optional<GuildEventTarget> EntityType;
    }
}
