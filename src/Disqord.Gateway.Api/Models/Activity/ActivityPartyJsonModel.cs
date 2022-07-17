using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Gateway.Api.Models;

public class ActivityPartyJsonModel : JsonModel
{
    [JsonProperty("id")]
    public Optional<string> Id;

    [JsonProperty("size")]
    public Optional<int[]> Size;
}