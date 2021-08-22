using System;
using Disqord.Serialization.Json;

namespace Disqord.Rest.Api
{
    public class CreateGuildEventJsonRestRequestContent : JsonModelRestRequestContent
    {
        [JsonProperty("channel_id")]
        public Optional<Snowflake> ChannelId;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("privacy_level")]
        public StagePrivacyLevel PrivacyLevel;

        [JsonProperty("scheduled_start_time")]
        public DateTimeOffset ScheduledStartTime;

        [JsonProperty("description")]
        public Optional<string> Description;

        [JsonProperty("entity_type")]
        public GuildScheduledEventEntityType EntityType;
    }
}