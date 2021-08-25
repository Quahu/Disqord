using System;

namespace Disqord
{
    public interface IMessageGuildChannel : ICategorizableGuildChannel, IMessageChannel, ITaggable
    {
        /// <summary>
        ///     Gets the slowmode of this channel.
        /// </summary>
        TimeSpan Slowmode { get; }
    }
}
