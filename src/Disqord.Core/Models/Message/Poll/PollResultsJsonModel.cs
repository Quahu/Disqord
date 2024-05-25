using Disqord.Serialization.Json;

namespace Disqord.Models;

public class PollResultsJsonModel : JsonModel
{
    [JsonProperty("is_finalized")]
    public bool IsFinalized;

    [JsonProperty("answer_counts")]
    public PollAnswerCountsJsonModel[] AnswerCounts = null!;
}
