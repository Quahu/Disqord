namespace Disqord.Voice.Api;

public enum VoiceGatewayPayloadOperation : byte
{
    Identify = 0,

    SelectProtocol = 1,

    Ready = 2,

    Heartbeat = 3,

    SessionDescription = 4,

    Speaking = 5,

    HeartbeatAcknowledged = 6,

    Resume = 7,

    Hello = 8,

    Resumed = 9,

    ClientConnect = 11,

    ClientDisconnect = 13,

    MediaSinkWants = 15,

    ClientFlags = 18,

    ChannelOptionsUpdate = 20,

    DaveProtocolPrepareTransition = 21,

    DaveProtocolExecuteTransition = 22,

    DaveProtocolTransitionReady = 23,

    DaveProtocolPrepareEpoch = 24,

    DaveMlsExternalSenderPackage = 25,

    DaveMlsKeyPackage = 26,

    DaveMlsProposals = 27,

    DaveMlsCommitWelcome = 28,

    DaveMlsAnnounceCommitTransition = 29,

    DaveMlsWelcome = 30,

    DaveMlsInvalidCommitWelcome = 31
}
