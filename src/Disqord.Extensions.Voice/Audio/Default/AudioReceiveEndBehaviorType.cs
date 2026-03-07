namespace Disqord.Extensions.Voice;

/// <summary>
///     Specifies how a receive subscription determines when to end.
/// </summary>
public enum AudioReceiveEndBehaviorType
{
    /// <summary>
    ///     The subscription remains open until explicitly disposed.
    /// </summary>
    Manual,

    /// <summary>
    ///     The subscription ends after a period of no packets (including silence).
    /// </summary>
    AfterInactivity,

    /// <summary>
    ///     The subscription ends after a period of only silence packets.
    /// </summary>
    AfterSilence,
}
