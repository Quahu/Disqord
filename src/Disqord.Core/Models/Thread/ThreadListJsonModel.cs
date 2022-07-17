using Disqord.Serialization.Json;

namespace Disqord.Models;

public class ThreadListJsonModel : JsonModel
{
    [JsonProperty("threads")]
    public ChannelJsonModel[] Threads = null!;

    [JsonProperty("members")]
    public ThreadMemberJsonModel[] Members = null!;

    [JsonProperty("has_more")]
    public bool HasMore;
}
