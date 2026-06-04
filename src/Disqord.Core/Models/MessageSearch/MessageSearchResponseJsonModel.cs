using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class MessageSearchResponseJsonModel : JsonModel
{
    [JsonProperty("analytics_id")]
    public string AnalyticsId = null!;

    [JsonProperty("messages")]
    public MessageJsonModel[][] Messages = null!;

    [JsonProperty("doing_deep_historical_index")]
    public bool DoingDeepHistoricalIndex;

    [JsonProperty("total_results")]
    public int TotalResults;

    [JsonProperty("threads")]
    public Optional<ChannelJsonModel[]> Threads;

    [JsonProperty("members")]
    public Optional<ThreadMemberJsonModel[]> Members;

    [JsonProperty("documents_indexed")]
    public Optional<int?> DocumentsIndexed;
}
