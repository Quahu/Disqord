using System;
using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class GuildScheduledEventJsonModel : JsonModel
    {
        [JsonProperty("id")]
        public Snowflake Id;

        [JsonProperty("guild_id")]
        public Snowflake GuildId;

        [JsonProperty("channel_id")]
        public Snowflake? ChannelId;

        [JsonProperty("creator_id")]
        public Snowflake? CreatorId;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("description")]
        public Optional<string> Description;

        [JsonProperty("image")]
        public string Image;

        [JsonProperty("scheduled_start_time")]
        public DateTimeOffset ScheduledStartTime;

        [JsonProperty("scheduled_end_time")]
        public DateTimeOffset? ScheduledEndTime;

        [JsonProperty("privacy_level")]
        public PrivacyLevel PrivacyLevel;

        [JsonProperty("status")]
        public GuildEventStatus Status;

        [JsonProperty("entity_type")]
        public GuildEventTargetType EntityType;

        [JsonProperty("entity_id")]
        public Snowflake? EntityId;

        [JsonProperty("entity_metadata")]
        public GuildScheduledEventEntityMetadataJsonModel EntityMetadata;

        [JsonProperty("creator")]
        public Optional<UserJsonModel> Creator;

        [JsonProperty("user_count")]
        public Optional<int> UserCount;
    }
}
