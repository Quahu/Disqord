namespace Disqord.Voice.Api.Models;

public enum VoiceGatewayPayloadOperation : byte
{
    Identify = 0,

    SelectProtocol = 1,

    Ready = 2,

    Heartbeat = 3,

    SessionDescription = 4,

    Speaking = 5,

    HeartbeatAck = 6,

    Resume = 7,

    Hello = 8,

    Resumed = 9,

    ClientConnect = 12,

    ClientDisconnect = 13
}