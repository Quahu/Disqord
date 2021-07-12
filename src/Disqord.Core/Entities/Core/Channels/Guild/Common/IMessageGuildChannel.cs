using System;

namespace Disqord
{
    public interface IMessageGuildChannel : ICategorizableGuildChannel, IMessageChannel
    {
        /// <summary>
        ///     Gets the slowmode of this channel.
        /// </summary>
        TimeSpan Slowmode { get; }
    }
}
