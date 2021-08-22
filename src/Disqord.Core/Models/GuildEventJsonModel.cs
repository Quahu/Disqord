using System;
using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class GuildEventJsonModel : JsonModel
    {
        [JsonProperty("id")]
        public Snowflake Id;

        [JsonProperty("guild_id")]
        public Snowflake GuildId;

        [JsonProperty("channel_id")]
        public Snowflake? ChannelId;

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
        public StagePrivacyLevel PrivacyLevel;

        [JsonProperty("status")]
        public GuildScheduledEventEntityType Status;

        [JsonProperty("entity_type")]
        public GuildScheduledEventStatus EntityType;

        [JsonProperty("entity_id")]
        public Snowflake? EntityId;

        [JsonProperty("entity_metadata")]
        public GuildEventEntityMetadataJsonModel EntityMetadata;

        [JsonProperty("sku_ids")]
        public Snowflake[] SkuIds;

        [JsonProperty("user_count")]
        public Optional<int> UserCount;
    }
}