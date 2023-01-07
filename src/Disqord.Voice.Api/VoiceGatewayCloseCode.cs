namespace Disqord.Voice.Api;

public enum VoiceGatewayCloseCode
{
    /// <summary>
    ///     The <see cref="VoiceGatewayPayloadOperation"/> specified was invalid.
    /// </summary>
    UnknownOperation = 4001,

    /// <summary>
    ///     The payload could not be decoded.
    /// </summary>
    DecodeError = 4002,

    /// <summary>
    ///     A payload was sent without prior authentication.
    /// </summary>
    NotAuthenticated = 4003,

    /// <summary>
    ///     The token provided was invalid.
    /// </summary>
    AuthenticationFailed = 4004,

    /// <summary>
    ///     A <see cref="VoiceGatewayPayloadOperation.Identify"/> was sent while already identified.
    /// </summary>
    AlreadyAuthenticated = 4005,

    /// <summary>
    ///     The gateway session is no longer valid.
    /// </summary>
    InvalidSession = 4006,

    /// <summary>
    ///     The gateway session timed out.
    /// </summary>
    SessionTimedOut = 4009,

    /// <summary>
    ///     The server being connected to was not found.
    /// </summary>
    ServerNotFound = 4011,

    /// <summary>
    ///     The protocol specified was not recognized.
    /// </summary>
    UnknownProtocol = 4012,

    /// <summary>
    ///     The connected to channel was deleted, the bot was kicked, the voice server changed, or the main session was dropped.
    /// </summary>
    ForciblyDisconnected = 4014,

    /// <summary>
    ///     The voice server crashed.
    /// </summary>
    ServerCrashed = 4015,

    /// <summary>
    ///     The encryption specified was not recognized.
    /// </summary>
    UnknownEncryption = 4016
}
