using System;

namespace Disqord
{
    /// <summary>
    ///     Represents a channel which can contain threads.
    /// </summary>
    public interface IThreadParentChannel : IAgeRestrictableChannel
    {
        /// <summary>
        ///     Gets the default automatic archive duration for threads created in this channel.
        /// </summary>
        TimeSpan DefaultAutomaticArchiveDuration { get; }
    }
}
