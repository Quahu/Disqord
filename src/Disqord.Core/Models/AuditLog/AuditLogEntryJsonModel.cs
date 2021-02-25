﻿using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class AuditLogEntryJsonModel : JsonModel
    {
        [JsonProperty("target_id")]
        public string TargetId = default!;

        [JsonProperty("changes")]
        public Optional<AuditLogChangeJsonModel[]> Changes;

        [JsonProperty("user_id")]
        public Snowflake UserId;

        [JsonProperty("id")]
        public Snowflake Id;

        [JsonProperty("action_type")]
        public AuditLogActionType ActionType;

        [JsonProperty("options")]
        public Optional<AuditLogEntryOptionsJsonModel> Options;

        [JsonProperty("reason")]
        public Optional<string> Reason;
    }
}