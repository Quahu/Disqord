using Disqord.Serialization.Json;

namespace Disqord.Voice.Api.Models;

public class ClientDisconnectJsonModel : JsonModel
{
    [JsonProperty("user_id")]
    public Snowflake UserId;
}
