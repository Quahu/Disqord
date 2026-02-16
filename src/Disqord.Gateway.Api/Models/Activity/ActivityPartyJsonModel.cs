using Disqord.Serialization.Json;
using Newtonsoft.Json;
using Qommon;

namespace Disqord.Gateway.Api.Models;

public class ActivityPartyJsonModel : JsonModel
{
    [Disqord.Serialization.Json.JsonProperty("id")]
    public Optional<string> Id;

    [Disqord.Serialization.Json.JsonProperty("size")]
    [JsonConverter(typeof(ActivityPartySizeJsonConverter))]
    public Optional<int[]> Size;
}
