using Disqord.Serialization.Json;

namespace Disqord.Models;

public class UnfurledMediaItemJsonModel : JsonModel
{
    [JsonProperty("url")]
    public string Url = null!;
}
