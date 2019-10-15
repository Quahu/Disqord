using Disqord.Rest;

namespace Disqord.Events
{
    public sealed class ReactionRemovedEventArgs : DiscordEventArgs
    {
        public ICachedMessageChannel Channel { get; }

        public DownloadableOptionalSnowflakeEntity<CachedUserMessage, RestMessage> Message { get; }

        public DownloadableOptionalSnowflakeEntity<CachedUser, RestUser> User { get; }

        public Optional<ReactionData> Reaction { get; }

        public IEmoji Emoji { get; }

        internal ReactionRemovedEventArgs(
            ICachedMessageChannel channel,
            DownloadableOptionalSnowflakeEntity<CachedUserMessage, RestMessage> message,
            DownloadableOptionalSnowflakeEntity<CachedUser, RestUser> user,
            Optional<ReactionData> reaction,
            IEmoji emoji) : base(channel.Client)
        {
            Channel = channel;
            Message = message;
            User = user;
            Reaction = reaction;
            Emoji = emoji;
        }
    }
}
