using System;
using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class GuildDiscoveryMetdataJsonModel : JsonModel
    {
        [JsonProperty("guild_id")] 
        public Snowflake GuildId;

        [JsonProperty("primary_category_id")] 
        public int PrimaryCategoryId;

        [JsonProperty("keywords")] 
        public string[] Keywords;

        [JsonProperty("emoji_discoverability_enabled")]
        public bool EmojiDiscoverabilityEnabled;

        [JsonProperty("partner_actioned_timestamp")]
        public DateTimeOffset? PartnerActionedTimestamp;

        [JsonProperty("partner_application_timestamp")]
        public DateTimeOffset? PartnerApplicationTimestamp;

        [JsonProperty("category_ids")]
        public int[] CategoryIds;
    }
}