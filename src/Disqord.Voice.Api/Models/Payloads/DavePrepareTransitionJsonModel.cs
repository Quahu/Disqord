using Disqord.Serialization.Json;

namespace Disqord.Voice.Api.Models;

public class DavePrepareTransitionJsonModel : JsonModel
{
    [JsonProperty("protocol_version")]
    public int ProtocolVersion;

    [JsonProperty("transition_id")]
    public int TransitionId;
}
