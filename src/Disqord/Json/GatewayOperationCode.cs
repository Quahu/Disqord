namespace Disqord.Models
{
    internal enum GatewayOperationCode : byte
    {
        Dispatch = 0,

        Heartbeat = 1,

        Identify = 2,

        StatusUpdate = 3,

        VoiceStateUpdate = 4,

        Resume = 6,

        Reconnect = 7,

        RequestGuildMembers = 8,

        InvalidSession = 9,

        Hello = 10,

        HeartbeatAck = 11,

        GuildSync = 12
    }
}