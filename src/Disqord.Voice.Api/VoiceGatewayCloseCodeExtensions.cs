namespace Disqord.Voice.Api;

public static class VoiceGatewayCloseCodeExtensions
{
    /// <summary>
    ///     Checks if the close code indicates that the voice gateway session can be resumed.
    /// </summary>
    /// <param name="code"> The code to check. </param>
    /// <returns>
    ///     <see langword="true"/> if the session can be resumed.
    /// </returns>
    public static bool IsResumable(this VoiceGatewayCloseCode code)
    {
        return code is not
            (VoiceGatewayCloseCode.AuthenticationFailed or
            VoiceGatewayCloseCode.InvalidSession or
            VoiceGatewayCloseCode.ServerNotFound or
            VoiceGatewayCloseCode.ServerCrashed or
            VoiceGatewayCloseCode.RateLimited or
            VoiceGatewayCloseCode.ForciblyDisconnected or
            VoiceGatewayCloseCode.CallTerminated);
    }
}
