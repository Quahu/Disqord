using Disqord.Serialization.Json;

namespace Disqord.Gateway.Api.Models;

public class GatewayPayloadJsonModel : JsonModel
{
    /// <summary>
    ///     The operation code of this payload.
    /// </summary>
    [JsonProperty("op")]
    public GatewayPayloadOperation Op;

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

    /// <summary>
    ///     The dispatch name of this payload. Only applicable to <see cref="GatewayPayloadOperation.Dispatch"/>.
    /// </summary>
    [JsonProperty("t")]
    public string? T;
}