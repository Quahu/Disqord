using System;

namespace Disqord.Extensions.Voice;

/// <summary>
///     Specifies options for an <see cref="AudioReceiverSubscription"/>.
/// </summary>
public class AudioReceiveSubscriptionOptions
{
    /// <summary>
    ///     Gets or sets the end behavior type for this subscription. Defaults to <see cref="AudioReceiveEndBehaviorType.Manual"/>.
    /// </summary>
    public AudioReceiveEndBehaviorType EndBehaviorType { get; set; } = AudioReceiveEndBehaviorType.Manual;

    /// <summary>
    ///     Gets or sets the duration used by <see cref="AudioReceiveEndBehaviorType.AfterInactivity"/>
    ///     and <see cref="AudioReceiveEndBehaviorType.AfterSilence"/> end behaviors.
    /// </summary>
    public TimeSpan EndBehaviorDuration { get; set; } = TimeSpan.FromMilliseconds(750);

    /// <summary>
    ///     Gets or sets the maximum amount of packet time buffered in memory.
    ///     Set to a value less than or equal to zero to disable buffering limits (unbounded).
    /// </summary>
    public TimeSpan MaxBufferedDuration { get; set; } = TimeSpan.FromSeconds(5);

    /// <summary>
    ///     Gets or sets how writes behave when the subscription buffer is full.
    /// </summary>
    public AudioReceiveBufferFullMode BufferFullMode { get; set; } = AudioReceiveBufferFullMode.DropOldest;
}
