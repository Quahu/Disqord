using Disqord.Serialization.Json;

namespace Disqord.Voice.Api.Models;

public class HeartbeatJsonModel : JsonModel
{
    [JsonProperty("t")]
    public long T;

    [JsonProperty("seq_ack")]
    public int SeqAck;
}
