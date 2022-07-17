using Disqord.Serialization.Json;

namespace Disqord.Models;

public class InviteStageInstanceJsonModel : JsonModel
{
    [JsonProperty("members")]
    public MemberJsonModel[] Members = null!;

    [JsonProperty("participant_count")]
    public int ParticipantCount;

    [JsonProperty("speaker_count")]
    public int SpeakerCount;

    [JsonProperty("topic")]
    public string Topic = null!;
}
