using System;
using Disqord.Serialization.Json;

namespace Disqord.Models;

public class GuildTemplateJsonModel : JsonModel
{
    [JsonProperty("code")]
    public string Code = null!;

    [JsonProperty("name")]
    public string Name = null!;

    [JsonProperty("description")]
    public string? Description;

    [JsonProperty("usage_count")]
    public int UsageCount;

    [JsonProperty("creator_id")]
    public Snowflake CreatorId;

    [JsonProperty("creator")]
    public UserJsonModel Creator = null!;

    [JsonProperty("created_at")]
    public DateTimeOffset CreatedAt;

    [JsonProperty("updated_at")]
    public DateTimeOffset UpdatedAt;

    [JsonProperty("source_guild_id")]
    public Snowflake SourceGuildId;

    [JsonProperty("serialized_source_guild")]
    public IJsonObject SerializedSourceGuild = null!;

    [JsonProperty("is_dirty")]
    public bool? IsDirty;
}
