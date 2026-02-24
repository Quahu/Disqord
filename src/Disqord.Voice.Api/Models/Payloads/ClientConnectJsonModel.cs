using Disqord.Serialization.Json;

namespace Disqord.Voice.Api.Models;

public class ClientConnectJsonModel : JsonModel
{
    [JsonProperty("user_ids")]
    public Snowflake[] UserIds = null!;
}
