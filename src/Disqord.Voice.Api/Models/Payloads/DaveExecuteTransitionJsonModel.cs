using Disqord.Serialization.Json;

namespace Disqord.Voice.Api.Models;

public class DaveExecuteTransitionJsonModel : JsonModel
{
    [JsonProperty("transition_id")]
    public int TransitionId;
}
