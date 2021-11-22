using System;
using Disqord.Models;
using Disqord.Serialization.Json;

namespace Disqord.Rest.Api
{
    public class CreateGuildScheduledEventJsonRestRequestContent : JsonModelRestRequestContent
    {
        [JsonProperty("channel_id")]
        public Optional<Snowflake> ChannelId;

        [JsonProperty("entity_metadata")]
        public Optional<GuildScheduledEventEntityMetadataJsonModel> EntityMetadata;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("privacy_level")]
        public PrivacyLevel PrivacyLevel;

        [JsonProperty("scheduled_start_time")]
        public DateTimeOffset ScheduledStartTime;

        [JsonProperty("scheduled_end_time")]
        public Optional<DateTimeOffset> ScheduledEndTime;

        [JsonProperty("description")]
        public Optional<string> Description;

        [JsonProperty("entity_type")]
        public GuildEventTargetType EntityType;
    }
}
