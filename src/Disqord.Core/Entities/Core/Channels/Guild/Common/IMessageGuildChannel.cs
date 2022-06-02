using System;

namespace Disqord
{
    /// <summary>
    ///     Represents a message channel in a guild.
    /// </summary>
    public interface IMessageGuildChannel : ICategorizableGuildChannel, IMessageChannel, ITaggableEntity
    {
        /// <summary>
        ///     Gets the slowmode of this channel.
        /// </summary>
        TimeSpan Slowmode { get; }
    }
}
