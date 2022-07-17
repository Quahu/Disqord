using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class MessageActivityJsonModel : JsonModel
{
    [JsonProperty("type")]
    public MessageActivityType Type;

    [JsonProperty("party_id")]
    public Optional<string> PartyId;
}