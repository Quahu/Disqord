using System.Collections.Generic;

namespace Disqord.Events
{
    public sealed class MessagesBulkDeletedEventArgs : DiscordEventArgs
    {
        public CachedTextChannel Channel { get; }

        public IReadOnlyList<OptionalSnowflakeEntity<CachedUserMessage>> Messages { get; }

        internal MessagesBulkDeletedEventArgs(CachedTextChannel channel,
            IReadOnlyList<OptionalSnowflakeEntity<CachedUserMessage>> messages) : base(channel.Client)
        {
            Channel = channel;
            Messages = messages;
        }
    }
}
