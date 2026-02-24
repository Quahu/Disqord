using Disqord.Serialization.Json;

namespace Disqord.Voice.Api.Models;

public class DaveTransitionReadyJsonModel : JsonModel
{
    [JsonProperty("transition_id")]
    public int TransitionId;
}
