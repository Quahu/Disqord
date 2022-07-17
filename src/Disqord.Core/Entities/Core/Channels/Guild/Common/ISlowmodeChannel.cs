using System;

namespace Disqord;

/// <summary>
///     Represents a guild channel that might have a slowmode.
/// </summary>
public interface ISlowmodeChannel : IGuildChannel
{
    /// <summary>
    ///     Gets the slowmode duration of this channel.
    /// </summary>
    TimeSpan Slowmode { get; }
}