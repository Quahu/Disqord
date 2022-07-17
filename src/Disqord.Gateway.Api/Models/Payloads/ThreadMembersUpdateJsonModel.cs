using Disqord.Models;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Gateway.Api.Models;

public class ThreadMembersUpdateJsonModel : JsonModel
{
    [JsonProperty("id")]
    public Snowflake Id;

    [JsonProperty("guild_id")]
    public Snowflake GuildId;

    [JsonProperty("member_count")]
    public int MemberCount;

    [JsonProperty("added_members")]
    public Optional<ThreadMemberJsonModel[]> AddedMembers;

    [JsonProperty("removed_member_ids")]
    public Optional<Snowflake[]> RemovedMemberIds;
}