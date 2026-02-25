using System;

namespace Disqord.Voice.Api.Models;

/// <summary>
///     Represents a voice gateway message that can be either a JSON payload or a binary DAVE payload.
/// </summary>
public readonly struct VoiceGatewayMessage
{
    /// <summary>
    ///     Gets the operation code of this message.
    /// </summary>
    public VoiceGatewayPayloadOperation Op { get; }

    /// <summary>
    ///     Gets the sequence number of this message, if present.
    /// </summary>
    public int? SequenceNumber { get; }

    /// <summary>
    ///     Gets whether this is a binary message.
    /// </summary>
    public bool IsBinary => JsonPayload == null;

    /// <summary>
    ///     Gets the JSON payload, if this is a JSON message.
    /// </summary>
    public VoiceGatewayPayloadJsonModel? JsonPayload { get; }

    /// <summary>
    ///     Gets the binary payload data (excluding the opcode/sequence header), if this is a binary message.
    /// </summary>
    public ReadOnlyMemory<byte> BinaryPayload { get; }

    private VoiceGatewayMessage(VoiceGatewayPayloadOperation op, int? sequenceNumber,
        VoiceGatewayPayloadJsonModel? jsonPayload, ReadOnlyMemory<byte> binaryPayload)
    {
        Op = op;
        SequenceNumber = sequenceNumber;
        JsonPayload = jsonPayload;
        BinaryPayload = binaryPayload;
    }

    /// <summary>
    ///     Creates a JSON voice gateway message.
    /// </summary>
    public static VoiceGatewayMessage FromJson(VoiceGatewayPayloadJsonModel payload)
    {
        return new VoiceGatewayMessage(payload.Op, payload.S, payload, binaryPayload: default);
    }

    /// <summary>
    ///     Creates a binary voice gateway message.
    /// </summary>
    public static VoiceGatewayMessage FromBinary(VoiceGatewayPayloadOperation op, int? sequenceNumber, ReadOnlyMemory<byte> payload)
    {
        return new VoiceGatewayMessage(op, sequenceNumber, jsonPayload: null, payload);
    }
}
