using System.Collections.Generic;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class ApplicationRoleConnectionMetadataJsonModel : JsonModel
{
    [JsonProperty("type")]
    public ApplicationRoleConnectionMetadataType Type;

    [JsonProperty("key")]
    public string Key = null!;

    [JsonProperty("name")]
    public string Name = null!;

    [JsonProperty("name_localizations")]
    public Optional<Dictionary<string, string>> NameLocalizations;

    [JsonProperty("description")]
    public string Description = null!;

    [JsonProperty("description_localizations")]
    public Optional<Dictionary<string, string>> DescriptionLocalizations;
}
