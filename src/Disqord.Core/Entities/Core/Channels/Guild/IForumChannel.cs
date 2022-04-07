using System;

namespace Disqord
{
    /// <summary>
    ///     Represents a guild forum channel.
    /// </summary>
    public interface IForumChannel : ICategorizableGuildChannel
    {
        /// <summary>
        ///     Gets the topic of this channel.
        /// </summary>
        string Topic { get; }

        /// <summary>
        ///     Gets whether this forum channel is not safe for work.
        /// </summary>
        bool IsNsfw { get; }

        /// <summary>
        ///     Gets the default automatic archive duration for threads created in this channel.
        /// </summary>
        TimeSpan DefaultAutomaticArchiveDuration { get; }
    }
}
