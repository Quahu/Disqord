using Disqord.Serialization.Json;

namespace Disqord.Voice.Api.Models;

public class SessionDescriptionJsonModel : JsonModel
{
    [JsonProperty("secret_key")]
    public byte[] SecretKey = null!;

    [JsonProperty("mode")]
    public string Mode = null!;
}