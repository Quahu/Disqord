using Disqord.Serialization.Json;

namespace Disqord.Gateway.Api.Models;

public class ResumeJsonModel : JsonModel
{
    [JsonProperty("token")]
    public string Token = null!;

    [JsonProperty("session_id")]
    public string SessionId = null!;

    [JsonProperty("seq")]
    public int? Seq;
}