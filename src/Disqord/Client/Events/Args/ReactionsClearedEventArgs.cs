using System.Collections.Generic;

namespace Disqord.Events
{
    public sealed class ReactionsClearedEventArgs : DiscordEventArgs
    {
        public ICachedMessageChannel Channel { get; }

        public OptionalSnowflakeEntity<CachedMessage> Message { get; }

        public Optional<IReadOnlyDictionary<IEmoji, ReactionData>> Reactions { get; }

        internal ReactionsClearedEventArgs(
            ICachedMessageChannel channel,
            OptionalSnowflakeEntity<CachedMessage> message,
            Optional<IReadOnlyDictionary<IEmoji, ReactionData>> reactions) : base(channel.Client)
        {
            Channel = channel;
            Message = message;
            Reactions = reactions;
        }
    }
}
