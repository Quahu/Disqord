using System;

namespace Disqord
{
    public interface IMessageGuildChannel : ICategorizableGuildChannel, IMessageChannel, ITaggableEntity
    {
        /// <summary>
        ///     Gets the slowmode of this channel.
        /// </summary>
        TimeSpan Slowmode { get; }
    }
}
