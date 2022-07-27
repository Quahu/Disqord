namespace Disqord.Gateway.Api;

public enum GatewayPayloadOperation
{
    Dispatch = 0,

    Heartbeat = 1,

    Identify = 2,

    UpdatePresence = 3,

    UpdateVoiceState = 4,

    Resume = 6,

    Reconnect = 7,

    RequestMembers = 8,

    InvalidSession = 9,

    Hello = 10,

    HeartbeatAcknowledged = 11
}
