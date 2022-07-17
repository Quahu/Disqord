namespace Disqord;

/// <summary>
///     Represents the status of a guild event.
/// </summary>
public enum GuildEventStatus
{
    /// <summary>
    ///     The event is scheduled.
    /// </summary>
    Scheduled = 1,

    /// <summary>
    ///     The event is currently active.
    /// </summary>
    Active = 2,

    /// <summary>
    ///     The event has been completed.
    /// </summary>
    Completed = 3,

    /// <summary>
    ///     The event has been canceled.
    /// </summary>
    Canceled = 4
}