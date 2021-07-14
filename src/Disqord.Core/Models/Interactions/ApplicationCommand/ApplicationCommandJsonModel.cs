﻿using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class ApplicationCommandJsonModel : JsonModel
    {
        [JsonProperty("id")]
        public Snowflake Id;

        [JsonProperty("application_id")]
        public Snowflake ApplicationId;

        [JsonProperty("guild_id")]
        public Optional<Snowflake> GuildId;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("description")]
        public string Description;

        [JsonProperty("options")]
        public Optional<ApplicationCommandOptionJsonModel[]> Options;

        [JsonProperty("default_permission")]
        public Optional<bool> DefaultPermission;
    }
}
