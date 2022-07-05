using System;

namespace Disqord;

public interface IThreadParentChannel : IAgeRestrictableChannel
{
    /// <summary>
    ///     Gets the default automatic archive duration for threads created in this channel.
    /// </summary>
    TimeSpan DefaultAutomaticArchiveDuration { get; }
}
