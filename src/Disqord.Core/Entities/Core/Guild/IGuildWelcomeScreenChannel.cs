namespace Disqord
{
    /// <summary>
    ///     Represents a welcome screen's channel.
    /// </summary>
    public interface IGuildWelcomeScreenChannel : IChannelEntity
    {
        /// <summary>
        ///     Gets the description of this welcome screen channel.
        /// </summary>
        string Description { get; }

        /// <summary>
        ///     Gets the emoji of this welcome screen channel.
        ///     This can be either a <see cref="ICustomEmoji"/> or a <see cref="IEmoji"/>.
        /// </summary>
        IEmoji Emoji { get; }
    }
}
