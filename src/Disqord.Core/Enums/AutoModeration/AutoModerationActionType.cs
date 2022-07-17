namespace Disqord;

/// <summary>
///     Represents the types of actions that can be performed when an auto-moderation rule is triggered.
/// </summary>
public enum AutoModerationActionType
{
    /// <summary>
    ///     Blocks the message sent.
    /// </summary>
    BlockMessage = 1,

    /// <summary>
    ///     Sends an alert to a specific channel.
    /// </summary>
    SendAlertMessage = 2,

    /// <summary>
    ///     Times-out the offending user.
    /// </summary>
    Timeout = 3
}