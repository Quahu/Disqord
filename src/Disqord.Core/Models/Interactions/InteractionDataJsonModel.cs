﻿using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class InteractionDataJsonModel : JsonModel
    {
        [JsonProperty("id")]
        public Optional<Snowflake> Id;

        [JsonProperty("name")]
        public Optional<string> Name;

        [JsonProperty("type")]
        public Optional<ApplicationCommandType> Type;

        [JsonProperty("resolved")]
        public Optional<ApplicationCommandInteractionDataResolvedJsonModel> Resolved;

        [JsonProperty("options")]
        public Optional<ApplicationCommandInteractionDataOptionJsonModel[]> Options;

        [JsonProperty("custom_id")]
        public Optional<string> CustomId;

        [JsonProperty("component_type")]
        public Optional<ComponentType> ComponentType;

        [JsonProperty("values")]
        public Optional<string[]> Values;

        [JsonProperty("target_id")]
        public Optional<Snowflake> TargetId;
    }
}
