using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class ApplicationCommandInteractionDataJsonModel : InteractionDataJsonModel
{
    [JsonProperty("id")]
    public Snowflake Id;

    [JsonProperty("name")]
    public string Name = null!;

    [JsonProperty("type")]
    public ApplicationCommandType Type;

    [JsonProperty("resolved")]
    public Optional<ApplicationCommandInteractionDataResolvedJsonModel> Resolved;

    [JsonProperty("options")]
    public Optional<ApplicationCommandInteractionDataOptionJsonModel[]> Options;

    [JsonProperty("guild_id")]
    public Optional<Snowflake> GuildId;

    [JsonProperty("target_id")]
    public Optional<Snowflake> TargetId;
}
