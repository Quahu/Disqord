using Disqord.Models;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Gateway.Api.Models;

public class ThreadListSyncJsonModel : JsonModel
{
    [JsonProperty("guild_id")]
    public Snowflake GuildId;

    [JsonProperty("channel_ids")]
    public Optional<Snowflake[]> ChannelIds;

    [JsonProperty("threads")]
    public ChannelJsonModel[] Threads = null!;

    [JsonProperty("members")]
    public ThreadMemberJsonModel[] Members = null!;
}