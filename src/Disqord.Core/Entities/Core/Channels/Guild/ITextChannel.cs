using System;

namespace Disqord
{
    /// <summary>
    ///     Represents a guild text channel.
    /// </summary>
    public interface ITextChannel : IMessageGuildChannel, IMentionable, ITaggable
    {
        /// <summary>
        ///     Gets the topic of this channel.
        /// </summary>
        string Topic { get; }

        /// <summary>
        ///     Gets whether this text channel is not safe for work.
        /// </summary>
        bool IsNsfw { get; }

        // TODO: With the addition of INewsChannel, is this made redundant?
        /// <summary>
        ///     Gets whether this text channel is a news channel.
        /// </summary>
        bool IsNews { get; }

        /// <summary>
        ///     Gets the default automatic archive duration for threads created in this channel.
        /// </summary>
        TimeSpan DefaultAutomaticArchiveDuration { get; }
    }
}
