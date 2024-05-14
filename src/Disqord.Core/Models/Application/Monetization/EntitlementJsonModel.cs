using System;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class EntitlementJsonModel : JsonModel
{
    [JsonProperty("id")]
    public Snowflake Id;

    [JsonProperty("type")]
    public EntitlementType Type;

    [JsonProperty("sku_id")]
    public Snowflake SkuId;

    [JsonProperty("application_id")]
    public Snowflake ApplicationId;

    [JsonProperty("user_id")]
    public Optional<Snowflake> UserId;

    [JsonProperty("deleted")]
    public bool Deleted;

    [JsonProperty("starts_at")]
    public DateTimeOffset? StartsAt;

    [JsonProperty("ends_at")]
    public DateTimeOffset? EndsAt;

    [JsonProperty("guild_id")]
    public Optional<Snowflake> GuildId;

    [JsonProperty("consumed")]
    public Optional<bool> Consumed;
}
