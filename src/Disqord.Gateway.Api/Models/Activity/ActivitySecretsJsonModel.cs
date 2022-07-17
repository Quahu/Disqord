using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Gateway.Api.Models;

public class ActivitySecretsJsonModel : JsonModel
{
    [JsonProperty("join")]
    public Optional<string> Join;

    [JsonProperty("spectate")]
    public Optional<string> Spectate;

    [JsonProperty("match")]
    public Optional<string> Match;
}