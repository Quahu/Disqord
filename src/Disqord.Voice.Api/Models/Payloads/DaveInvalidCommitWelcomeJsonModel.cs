using Disqord.Serialization.Json;

namespace Disqord.Voice.Api.Models;

public class DaveInvalidCommitWelcomeJsonModel : JsonModel
{
    [JsonProperty("transition_id")]
    public int TransitionId;
}
