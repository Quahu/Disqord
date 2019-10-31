using System.Collections.Generic;

namespace Disqord
{
    public interface ICachedMessageChannel : IMessageChannel
    {
        new DiscordClientBase Client { get; }

        /// <summary>
        ///     Attempts to get a message from the message cache.
        /// </summary>
        /// <param name="id"> The id of the message. </param>
        /// <returns>
        ///     A <see cref="CachedUserMessage"/> or <see langword="null"/> if it was not cached.
        /// </returns>
        CachedUserMessage GetMessage(Snowflake id);

        /// <summary>
        ///     Attempts to get messages from the message cache.
        /// </summary>
        /// <returns>
        ///     An <see cref="IReadOnlyList{T}"/> of <see cref="CachedUserMessage"/>s or <see langword="null"/> if it was not cached.
        /// </returns>
        IReadOnlyList<CachedUserMessage> GetMessages();
    }
}
