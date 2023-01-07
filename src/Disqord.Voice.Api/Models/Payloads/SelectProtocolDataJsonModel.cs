using Disqord.Serialization.Json;

namespace Disqord.Voice.Api.Models;

public class SelectProtocolDataJsonModel : JsonModel
{
    [JsonProperty("address")]
    public string Address = null!;

    [JsonProperty("port")]
    public int Port;

    [JsonProperty("mode")]
    public string Mode = null!;
}
