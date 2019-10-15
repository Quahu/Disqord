using System.Collections.Generic;

namespace Disqord
{
    public interface ICachedMessageChannel : IMessageChannel
    {
        new DiscordClient Client { get; }

        /// <summary>
        ///     Gets the collection of cached messages.
        /// </summary>
        IReadOnlyList<CachedUserMessage> CachedMessages { get; }

        /// <summary>
        ///     Attempts to get a message by its id from the cache.
        /// </summary>
        /// <returns>
        ///     A <see cref="CachedUserMessage"/> or <see langword="null"/> if it's not found.
        /// </returns>
        CachedUserMessage GetMessage(Snowflake id);
    }
}
