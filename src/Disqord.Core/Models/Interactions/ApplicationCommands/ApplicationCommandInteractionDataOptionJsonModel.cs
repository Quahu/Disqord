using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class ApplicationCommandInteractionDataOptionJsonModel : JsonModel
{
    [JsonProperty("name")]
    public string Name = null!;

    [JsonProperty("type")]
    public SlashCommandOptionType Type;

    [JsonProperty("value")]
    public Optional<IJsonValue> Value;

    [JsonProperty("options")]
    public Optional<ApplicationCommandInteractionDataOptionJsonModel[]> Options;

    [JsonProperty("focused")]
    public Optional<bool> Focused;
}
