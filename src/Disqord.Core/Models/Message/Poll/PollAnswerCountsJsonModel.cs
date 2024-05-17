using Disqord.Serialization.Json;

namespace Disqord.Models;

public class PollAnswerCountsJsonModel : JsonModel
{
    [JsonProperty("id")]
    public int Id;

    [JsonProperty("count")]
    public int Count;

    [JsonProperty("me_voted")]
    public bool MeVoted;
}
