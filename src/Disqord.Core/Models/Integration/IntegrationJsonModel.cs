using System;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class IntegrationJsonModel : JsonModel
{
    [JsonProperty("id")]
    public Snowflake Id;

    [JsonProperty("name")]
    public string Name = null!;

    [JsonProperty("type")]
    public string Type = null!;

    [JsonProperty("enabled")]
    public Optional<bool> Enabled;

    [JsonProperty("syncing")]
    public Optional<bool> Syncing;

    [JsonProperty("role_id")]
    public Optional<Snowflake> RoleId;

    [JsonProperty("enable_emoticons")]
    public Optional<bool> EnableEmoticons;

    [JsonProperty("expire_behavior")]
    public Optional<IntegrationExpirationBehavior> ExpireBehavior;

    [JsonProperty("expire_grace_period")]
    public Optional<int> ExpireGracePeriod;

    [JsonProperty("user")]
    public Optional<UserJsonModel> User;

    [JsonProperty("account")]
    public IntegrationAccountJsonModel Account = null!;

    [JsonProperty("synced_at")]
    public Optional<DateTimeOffset> SyncedAt;

    [JsonProperty("subscriber_count")]
    public Optional<int> SubscriberCount;

    [JsonProperty("revoked")]
    public Optional<bool> Revoked;

    [JsonProperty("application")]
    public Optional<IntegrationApplicationJsonModel> Application;
}
