using System.Collections.Generic;
using System.ComponentModel;

namespace Disqord
{
    /// <summary>
    ///     Represents a dummy cache that never caches anything.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public sealed class DummyMessageCache : MessageCache
    {
        /// <summary>
        ///     The singleton instance of <see cref="DummyMessageCache"/>.
        /// </summary>
        public static readonly DummyMessageCache Instance = new DummyMessageCache();

        private DummyMessageCache()
        { }

        public override bool TryAddMessage(CachedUserMessage message)
            => false;

        /// <inheritdoc/>
        public override bool TryGetMessage(Snowflake channelId, Snowflake messageId, out CachedUserMessage message)
        {
            message = null;
            return false;
        }

        /// <inheritdoc/>
        public override bool TryGetMessages(Snowflake channelId, out IEnumerable<CachedUserMessage> messages)
        {
            messages = null;
            return false;
        }

        /// <inheritdoc/>
        public override bool TryRemoveMessage(Snowflake channelId, Snowflake messageId, out CachedUserMessage message)
        {
            message = null;
            return false;
        }

        /// <inheritdoc/>
        public override bool TryRemoveMessages(Snowflake channelId, out IEnumerable<CachedUserMessage> messages)
        {
            messages = null;
            return false;
        }

        /// <inheritdoc/>
        public override void Clear()
        { }
    }
}
