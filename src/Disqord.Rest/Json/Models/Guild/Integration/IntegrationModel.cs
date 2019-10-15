using System;
using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal sealed class IntegrationModel
    {
        [JsonProperty("id")]
        public ulong Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("string")]
        public string Type { get; set; }

        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("syncing")]
        public bool Syncing { get; set; }

        [JsonProperty("role_id")]
        public ulong RoleId { get; set; }

        [JsonProperty("expire_behavior")]
        public int ExpireBehavior { get; set; }

        [JsonProperty("expire_grace_period")]
        public int ExpireGracePeriod { get; set; }

        [JsonProperty("user")]
        public UserModel User { get; set; }

        [JsonProperty("account")]
        public IntegrationAccountModel Account { get; set; }

        [JsonProperty("synced_at")]
        public DateTimeOffset SyncedAt { get; set; }
    }
}
