namespace Disqord.Events
{
    public sealed class EmojiReactionsClearedEventArgs : DiscordEventArgs
    {
        public CachedTextChannel Channel { get; }

        public FetchableSnowflakeOptional<IMessage> Message { get; }

        /// <summary>
        ///     Gets the cleared <see cref="IEmoji"/>.
        /// </summary>
        public IEmoji Emoji { get; }

        /// <summary>
        ///     Gets the cleared reaction data.
        ///     If the message wasn't cached, was cached at a later point this might return <see langword="null"/>.
        /// </summary>
        public ReactionData Data { get; }

        internal EmojiReactionsClearedEventArgs(
            CachedTextChannel channel,
            FetchableSnowflakeOptional<IMessage> message,
            IEmoji emoji,
            ReactionData data) : base(channel.Client)
        {
            Channel = channel;
            Message = message;
            Emoji = emoji;
            Data = data;
        }
    }
}
