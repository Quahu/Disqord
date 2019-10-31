using System.Collections.Generic;

namespace Disqord
{
    /// <summary>
    ///     Represents the base class for message caches. Implementations of this class should be thread-safe.
    /// </summary>
    public abstract class MessageCache
    {
        /// <summary>
        ///     Instiantiates a new <see cref="MessageCache"/>.
        /// </summary>
        protected MessageCache()
        { }

        /// <summary>
        ///     Attempts to cache the given message.
        /// </summary>
        /// <param name="message"> The message to cache. </param>
        /// <returns>
        ///     A <see cref="bool"/> indicating whether the message was cached.
        /// </returns>
        public abstract bool TryAddMessage(CachedUserMessage message);

        /// <summary>
        ///     Attempts to get a message from the cache with the specified id, for the specified channel.
        /// </summary>
        /// <param name="channelId"> The id of the message's channel. </param>
        /// <param name="messageId"> The id of the message. </param>
        /// <param name="message"> The optional cached message. </param>
        /// <returns>
        ///     A <see cref="bool"/> indicating whether the message was present in the cache.
        /// </returns>
        public abstract bool TryGetMessage(Snowflake channelId, Snowflake messageId, out CachedUserMessage message);

        /// <summary>
        ///     Attempts to get messages from the cache for the specified channel.
        /// </summary>
        /// <param name="channelId"> The id of the channel. </param>
        /// <param name="messages"> The optional cached messages. </param>
        /// <returns>
        ///     A <see cref="bool"/> indicating whether the message was present in the cache.
        /// </returns>
        public abstract bool TryGetMessages(Snowflake channelId, out IEnumerable<CachedUserMessage> messages);

        /// <summary>
        ///     Attempts to remove a message from the cache with the specified id, for the specified channel.
        /// </summary>
        /// <param name="channelId"> The id of the message's channel. </param>
        /// <param name="messageId"> The id of the message. </param>
        /// <param name="message"> The optional cached message. </param>
        /// <returns>
        ///     A <see cref="bool"/> indicating whether the message was present in the cache.
        /// </returns>
        public abstract bool TryRemoveMessage(Snowflake channelId, Snowflake messageId, out CachedUserMessage message);

        /// <summary>
        ///     Attempts to remove messages from the cache for the specified channel.
        /// </summary>
        /// <param name="channelId"> The id of the channel. </param>
        /// <param name="messages"> The cleared cached messages. </param>
        /// <returns>
        ///     A <see cref="bool"/> indicating whether any messages were cleared.
        /// </returns>
        public abstract bool TryRemoveMessages(Snowflake channelId, out IEnumerable<CachedUserMessage> messages);

        /// <summary>
        ///     Clears the entire cache.
        /// </summary>
        public abstract void Clear();
    }
}
