using Disqord.Serialization.Json;

namespace Disqord.Voice.Api.Models;

public class VoiceGatewayPayloadJsonModel : JsonModel
{
    /// <summary>
    ///     The operation code of this payload.
    /// </summary>
    [JsonProperty("op")]
    public VoiceGatewayPayloadOperation Op;

    /// <summary>
    ///     The JSON token (any) representing the data of this payload.
    /// </summary>
    [JsonProperty("d")]
    public IJsonNode? D;

    /// <summary>
    ///     The sequence number of this payload.
    /// </summary>
    [JsonProperty("s")]
    public int? S;
}