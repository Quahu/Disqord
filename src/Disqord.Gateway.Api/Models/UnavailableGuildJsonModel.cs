using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Gateway.Api.Models;

public class UnavailableGuildJsonModel : JsonModel
{
    [JsonProperty("id")]
    public Snowflake Id;

    [JsonProperty("unavailable")]
    public Optional<bool> Unavailable;
}