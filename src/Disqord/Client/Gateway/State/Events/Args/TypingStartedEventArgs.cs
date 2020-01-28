using System;

namespace Disqord.Events
{
    public sealed class TypingStartedEventArgs : DiscordEventArgs
    {
        public SnowflakeOptional<ICachedMessageChannel> Channel { get; }

        public FetchableSnowflakeOptional<IUser> User { get; }

        public DateTimeOffset Timestamp { get; }

        internal TypingStartedEventArgs(DiscordClientBase client,
            SnowflakeOptional<ICachedMessageChannel> channel,
            FetchableSnowflakeOptional<IUser> user,
            DateTimeOffset timestamp) : base(client)
        {
            Channel = channel;
            User = user;
            Timestamp = timestamp;
        }
    }
}
