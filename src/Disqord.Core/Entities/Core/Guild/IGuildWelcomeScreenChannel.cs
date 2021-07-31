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
        ///     Gets the ID of the emoji of this welcome screen channel.
        ///     Returns <see langword="null"/> if the emoji is not custom.
        /// </summary>
        Snowflake? EmojiId { get; }

        /// <summary>
        ///     Gets the name of the emoji of this welcome screen channel.
        /// </summary>
        string EmojiName { get; }
    }
}
