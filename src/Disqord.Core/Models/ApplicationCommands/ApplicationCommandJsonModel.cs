using System.Collections.Generic;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class ApplicationCommandJsonModel : JsonModel
{
    [JsonProperty("id")]
    public Snowflake Id;

    [JsonProperty("type")]
    public Optional<ApplicationCommandType> Type;

    [JsonProperty("application_id")]
    public Snowflake ApplicationId;

    [JsonProperty("guild_id")]
    public Optional<Snowflake> GuildId;

    [JsonProperty("name")]
    public string Name = null!;

    [JsonProperty("name_localizations")]
    public Optional<Dictionary<string, string>?> NameLocalizations;

    [JsonProperty("description")]
    public string Description = null!;

    [JsonProperty("description_localizations")]
    public Optional<Dictionary<string, string>?> DescriptionLocalizations;

    [JsonProperty("options")]
    public Optional<ApplicationCommandOptionJsonModel[]> Options;

    [JsonProperty("default_member_permissions")]
    public Permissions? DefaultMemberPermissions;

    [JsonProperty("dm_permission")]
    public Optional<bool> DmPermission;

    [JsonProperty("default_permission")]
    public Optional<bool?> DefaultPermission;

    [JsonProperty("version")]
    public Snowflake Version;
}
