using Disqord.Serialization.Json;

namespace Disqord.Models;

public class InstallParamsJsonModel : JsonModel
{
    [JsonProperty("scopes")]
    public string[] Scopes = null!;

    [JsonProperty("permissions")]
    public Permissions Permissions;
}
