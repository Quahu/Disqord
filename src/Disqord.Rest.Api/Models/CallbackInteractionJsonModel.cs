using Disqord.Serialization.Json;

namespace Disqord.Rest.Api.Models;

public class CallbackInteractionJsonModel : JsonModel
{
    [JsonProperty("id")]
    public Snowflake Id;

    [JsonProperty("type")]
    public InteractionType Type;

    [JsonProperty("response_message_id")]
    public Snowflake? ResponseMessageId;

    [JsonProperty("response_message_loading")]
    public bool? ResponseMessageLoading;

    [JsonProperty("response_message_ephemeral")]
    public bool? ResponseMessageEphemeral;
}
