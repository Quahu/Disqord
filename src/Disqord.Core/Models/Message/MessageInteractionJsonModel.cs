using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class MessageInteractionJsonModel : JsonModel
{
    [JsonProperty("id")]
    public Snowflake Id;

    [JsonProperty("type")]
    public InteractionType Type;

    [JsonProperty("name")]
    public string Name = null!;

    [JsonProperty("user")]
    public UserJsonModel User = null!;

    [JsonProperty("member")]
    public Optional<MemberJsonModel> Member;
}
