using Disqord.Serialization.Json;

namespace Disqord.Voice.Api.Models;

public class SelectProtocolJsonModel : JsonModel
{
    [JsonProperty("protocol")]
    public string Protocol = null!;

    [JsonProperty("data")]
    public SelectProtocolDataJsonModel Data = null!;
}
