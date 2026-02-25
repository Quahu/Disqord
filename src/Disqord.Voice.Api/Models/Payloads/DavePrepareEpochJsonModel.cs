using Disqord.Serialization.Json;

namespace Disqord.Voice.Api.Models;

public class DavePrepareEpochJsonModel : JsonModel
{
    [JsonProperty("protocol_version")]
    public int ProtocolVersion;

    [JsonProperty("epoch")]
    public int Epoch;
}
