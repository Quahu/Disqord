using System;
using Disqord.Rest;

namespace Disqord.Events
{
    public sealed class TypingStartedEventArgs : DiscordEventArgs
    {
        public OptionalSnowflakeEntity<ICachedMessageChannel> Channel { get; }

        public DownloadableOptionalSnowflakeEntity<CachedUser, RestUser> User { get; }

        public DateTimeOffset Timestamp { get; }

        internal TypingStartedEventArgs(DiscordClientBase client,
            OptionalSnowflakeEntity<ICachedMessageChannel> channel,
            DownloadableOptionalSnowflakeEntity<CachedUser, RestUser> user,
            DateTimeOffset timestamp) : base(client)
        {
            Channel = channel;
            User = user;
            Timestamp = timestamp;
        }
    }
}
