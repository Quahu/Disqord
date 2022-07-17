using Disqord.Serialization.Json;

namespace Disqord.Voice.Api.Models;

public class ReadyJsonModel : JsonModel
{
    [JsonProperty("ssrc")]
    public uint Ssrc;

    [JsonProperty("ip")]
    public string Ip = null!;

    [JsonProperty("port")]
    public int Port;

    [JsonProperty("modes")]
    public string[] Modes = null!;
}