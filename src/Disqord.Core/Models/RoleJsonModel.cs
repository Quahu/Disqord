using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

[JsonSkippedProperties("flags")]
public class RoleJsonModel : JsonModel
{
    [JsonProperty("id")]
    public Snowflake Id;

    [JsonProperty("name")]
    public string Name = null!;

    [JsonProperty("color")]
    public int Color;

    [JsonProperty("hoist")]
    public bool Hoist;

    [JsonProperty("icon")]
    public Optional<string?> Icon;

    [JsonProperty("unicode_emoji")]
    public Optional<string?> UnicodeEmoji;

    [JsonProperty("position")]
    public int Position;

    [JsonProperty("permissions")]
    public Permissions Permissions;

    [JsonProperty("managed")]
    public bool Managed;

    [JsonProperty("mentionable")]
    public bool Mentionable;

    [JsonProperty("tags")]
    public Optional<RoleTagsJsonModel> Tags;
}
