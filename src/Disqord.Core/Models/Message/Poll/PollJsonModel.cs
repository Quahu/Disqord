using System;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class PollJsonModel : JsonModel
{
    [JsonProperty("question")]
    public PollMediaJsonModel Question = null!;

    [JsonProperty("answers")]
    public PollAnswerJsonModel[] Answers = null!;

    [JsonProperty("expiry")]
    public Optional<DateTimeOffset?> Expiry;

    [JsonProperty("duration")]
    public Optional<int> Duration;

    [JsonProperty("allow_multiselect")]
    public bool AllowMultiselect;

    [JsonProperty("layout_type")]
    public PollLayoutType LayoutType;

    [JsonProperty("results")]
    public Optional<PollResultsJsonModel> Results;
}
