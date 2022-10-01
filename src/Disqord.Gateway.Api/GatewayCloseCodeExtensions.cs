namespace Disqord.Gateway.Api;

public static class GatewayCloseCodeExtensions
{
    /// <summary>
    ///     Checks if the close code indicates a recoverable gateway connection.
    /// </summary>
    /// <remarks>
    ///     A gateway connection is recoverable if the close code is not one of:
    ///     <see cref="GatewayCloseCode.AuthenticationFailed"/>, <see cref="GatewayCloseCode.InvalidShard"/>,
    ///     <see cref="GatewayCloseCode.ShardingRequired"/>, <see cref="GatewayCloseCode.InvalidApiVersion"/>,
    ///     <see cref="GatewayCloseCode.InvalidIntents"/>, or <see cref="GatewayCloseCode.DisallowedIntents"/>.
    /// </remarks>
    /// <param name="code"> The code to check. </param>
    /// <returns>
    ///     <see langword="true"/> if the connection is recoverable.
    /// </returns>
    public static bool IsRecoverable(this GatewayCloseCode code)
    {
        return code is not
            (GatewayCloseCode.AuthenticationFailed or
            GatewayCloseCode.InvalidShard or
            GatewayCloseCode.ShardingRequired or
            GatewayCloseCode.InvalidApiVersion or
            GatewayCloseCode.InvalidIntents or
            GatewayCloseCode.DisallowedIntents);
    }
}
