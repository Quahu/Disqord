using System.Collections.Generic;
using Disqord.Serialization.Json;

namespace Disqord.Models;

public class ApplicationRoleConnectionJsonModel : JsonModel
{
    [JsonProperty("platform_name")]
    public string? PlatformName;

    [JsonProperty("platform_username")]
    public string? PlatformUsername;

    [JsonProperty("metadata")]
    public Dictionary<string, string> Metadata = new();
}
