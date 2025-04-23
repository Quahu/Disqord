using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class PollAnswerJsonModel : JsonModel
{
    [JsonProperty("answer_id")]
    public Optional<int> AnswerId;

    [JsonProperty("poll_media")]
    public PollMediaJsonModel PollMedia = null!;
}
