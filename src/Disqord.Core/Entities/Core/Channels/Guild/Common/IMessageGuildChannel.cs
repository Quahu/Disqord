using System;

namespace Disqord
{
    public interface IMessageGuildChannel : ICategorizableGuildChannel, IMessageChannel, IMentionable, ITaggable
    {
        /// <summary>
        ///     Gets the slowmode of this channel.
        /// </summary>
        TimeSpan Slowmode { get; }
    }
}
