using System;
using Disqord.Models;
using Disqord.Serialization.Json;

namespace Disqord.Rest.Api
{
    public class ModifyGuildScheduledEventJsonRestRequestContent : JsonModelRestRequestContent
    {
        [JsonProperty("channel_id")]
        public Optional<Snowflake> ChannelId;

        [JsonProperty("entity_metadata")]
        public Optional<GuildScheduledEventEntityMetadataJsonModel> EntityMetadata;

        [JsonProperty("name")]
        public Optional<string> Name;

        [JsonProperty("privacy_level")]
        public Optional<PrivacyLevel> PrivacyLevel;

        [JsonProperty("scheduled_start_time")]
        public Optional<DateTimeOffset> ScheduledStartTime;

        [JsonProperty("scheduled_end_time")]
        public Optional<DateTimeOffset> ScheduledEndTime;

        [JsonProperty("description")]
        public Optional<string> Description;

        [JsonProperty("entity_type")]
        public Optional<GuildEventTargetType> EntityType;

        [JsonProperty("status")]
        public Optional<GuildEventStatus> Status;
    }
}
