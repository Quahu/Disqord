using Disqord.Models;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Gateway.Api.Models;

public class GuildMembersChunkJsonModel : JsonModel
{
    [JsonProperty("guild_id")]
    public Snowflake GuildId;

    [JsonProperty("members")]
    public MemberJsonModel[] Members = null!;

    [JsonProperty("chunk_index")]
    public int ChunkIndex;

    [JsonProperty("chunk_count")]
    public int ChunkCount;

    [JsonProperty("not_found")]
    public Optional<Snowflake[]> NotFound;

    [JsonProperty("presences")]
    public Optional<PresenceJsonModel[]> Presences;

    [JsonProperty("nonce")]
    public Optional<string> Nonce;
}