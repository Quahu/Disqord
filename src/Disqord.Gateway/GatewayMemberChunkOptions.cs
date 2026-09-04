namespace Disqord.Gateway;

/// <summary>
///     Represents the options for a single member chunk or query operation.
/// </summary>
public class GatewayMemberChunkOptions
{
    /// <summary>
    ///     Gets or sets the maximum number of failed attempts before the operation is abandoned.
    ///     Defaults to <see langword="null"/>, i.e., <seealso cref="IGatewayChunker"/>'s configured default.
    /// </summary>
    public int? MaxAttempts { get; set; }
}
