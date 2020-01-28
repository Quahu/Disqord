using System.Collections.Generic;
using Disqord.Collections;

namespace Disqord
{
    /// <summary>
    ///     Represents a per-channel circular cache of the given capacity.
    /// </summary>
    public sealed class DefaultMessageCache : MessageCache
    {
        /// <summary>
        ///     Gets the capacity of each channel's circular cache.
        /// </summary>
        public int Capacity { get; }

        private readonly LockedDictionary<Snowflake, CircularBuffer<CachedUserMessage>> _caches;

        /// <summary>
        ///     Instiantiates a new <see cref="DefaultMessageCache"/> with the specified capacity.
        /// </summary>
        /// <param name="capacity"> The capacity of each channel's circular cache. </param>
        public DefaultMessageCache(int capacity)
        {
            Capacity = capacity;
            _caches = new LockedDictionary<Snowflake, CircularBuffer<CachedUserMessage>>();
        }

        /// <summary>
        ///     Attempts to cache the given message.
        /// </summary>
        /// <param name="message"> The message to cache. </param>
        /// <returns>
        ///     A <see cref="bool"/> indicating whether the message was cached.
        /// </returns>
        public override bool TryAddMessage(CachedUserMessage message)
        {
            var cache = _caches.GetOrAdd(message.Channel.Id, (_, capacity) => new CircularBuffer<CachedUserMessage>(capacity), Capacity);
            cache.Add(message);
            return true;
        }

        /// <summary>
        ///     Attempts to fetch a message from the cache with the specified id, for the specified channel.
        /// </summary>
        /// <param name="channelId"> The message's channel's id. </param>
        /// <param name="messageId"> The id of the message. </param>
        /// <param name="message"> The optional cached message. </param>
        /// <returns>
        ///     A <see cref="bool"/> indicating whether the message was present in the cache.
        /// </returns>
        public override bool TryGetMessage(Snowflake channelId, Snowflake messageId, out CachedUserMessage message)
        {
            if (!_caches.TryGetValue(channelId, out var cache))
            {
                message = null;
                return false;
            }

            message = cache.Find(x => x.Id == messageId);
            return message != null;
        }

        /// <summary>
        ///     Attempts to fetch messages from the cache for the specified channel.
        /// </summary>
        /// <param name="channelId"> The id of the channel. </param>
        /// <param name="messages"> The optional cached messages. </param>
        /// <returns>
        ///     A <see cref="bool"/> indicating whether the message was present in the cache.
        /// </returns>
        public override bool TryGetMessages(Snowflake channelId, out IEnumerable<CachedUserMessage> messages)
        {
            if (!_caches.TryGetValue(channelId, out var cache))
            {
                messages = null;
                return false;
            }

            messages = cache;
            return true;
        }

        /// <summary>
        ///     Attempts to remove a message from the cache with the specified id, for the specified channel.
        /// </summary>
        /// <param name="channelId"> The message's channel's id. </param>
        /// <param name="messageId"> The id of the message. </param>
        /// <param name="message"> The optional cached message. </param>
        /// <returns>
        ///     A <see cref="bool"/> indicating whether the message was present in the cache.
        /// </returns>
        public override bool TryRemoveMessage(Snowflake channelId, Snowflake messageId, out CachedUserMessage message)
        {
            if (!_caches.TryGetValue(channelId, out var cache) || !cache.TryRemove(x => x.Id == messageId, out message))
            {
                message = null;
                return false;
            }

            return true;
        }

        /// <summary>
        ///     Attempts to clear the cache for the specified channel.
        /// </summary>
        /// <param name="channelId"> The channel's id to clear the cache for. </param>
        /// <param name="messages"> The cleared cached messages. </param>
        /// <returns>
        ///     A <see cref="bool"/> indicating whether any messages were cleared.
        /// </returns>
        public override bool TryRemoveMessages(Snowflake channelId, out IEnumerable<CachedUserMessage> messages)
        {
            if (!_caches.TryRemove(channelId, out var cache))
            {
                messages = null;
                return false;
            }

            messages = cache.ReadOnly();
            return true;
        }

        /// <summary>
        ///     Clears the entire cache.
        /// </summary>
        public override void Clear()
        {
            _caches.Clear();
        }
    }
}
