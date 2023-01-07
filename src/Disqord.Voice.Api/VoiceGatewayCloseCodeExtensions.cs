namespace Disqord.Voice.Api;

public static class VoiceGatewayCloseCodeExtensions
{
    /// <summary>
    ///     Checks if the close code indicates a recoverable voice gateway connection.
    /// </summary>
    /// <param name="code"> The code to check. </param>
    /// <returns>
    ///     <see langword="true"/> if the connection is recoverable.
    /// </returns>
    public static bool IsRecoverable(this VoiceGatewayCloseCode code)
    {
        return code is not
            (VoiceGatewayCloseCode.UnknownOperation or
            VoiceGatewayCloseCode.DecodeError or
            VoiceGatewayCloseCode.NotAuthenticated or
            VoiceGatewayCloseCode.AuthenticationFailed or
            VoiceGatewayCloseCode.AlreadyAuthenticated or
            VoiceGatewayCloseCode.InvalidSession or
            VoiceGatewayCloseCode.SessionTimedOut or
            VoiceGatewayCloseCode.ServerNotFound or
            VoiceGatewayCloseCode.UnknownProtocol or
            VoiceGatewayCloseCode.ForciblyDisconnected or
            VoiceGatewayCloseCode.UnknownEncryption);
    }
}
