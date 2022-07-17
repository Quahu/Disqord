using Disqord.Serialization.Json;

namespace Disqord.Models;

public class IntegrationAccountJsonModel : JsonModel
{
    [JsonProperty("id")]
    public string Id = null!;

    [JsonProperty("name")]
    public string Name = null!;
}
