namespace Disqord.Extensions.Voice;

/// <summary>
///     Specifies how the subscription buffer behaves when full.
/// </summary>
public enum AudioReceiveBufferFullMode
{
    /// <summary>
    ///     Drop the oldest buffered packet to make room for the new one.
    /// </summary>
    DropOldest,

    /// <summary>
    ///     Backpressure the producer until space is available in the buffer.
    /// </summary>
    Wait,

    /// <summary>
    ///     Drop the newest incoming packet when the buffer is full.
    /// </summary>
    DropNewest,
}
