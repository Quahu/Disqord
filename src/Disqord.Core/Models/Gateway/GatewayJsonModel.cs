using Disqord.Serialization.Json;

namespace Disqord.Models;

public class GatewayJsonModel : JsonModel
{
    [JsonProperty("url")]
    public string Url = null!;
}