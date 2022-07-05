using System;

namespace Disqord;

public interface ISlowmodeChannel : IGuildChannel
{
    /// <summary>
    ///     Gets the slowmode duration of this channel.
    /// </summary>
    TimeSpan Slowmode { get; }
}
