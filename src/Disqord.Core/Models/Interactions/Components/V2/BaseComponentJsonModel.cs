using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class BaseComponentJsonModel : JsonModel
{
    [JsonProperty("type")]
    public ComponentType Type;

    [JsonProperty("id")]
    public Optional<int> Id;
}
