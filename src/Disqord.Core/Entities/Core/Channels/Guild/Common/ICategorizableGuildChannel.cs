namespace Disqord
{
    /// <summary>
    ///     Represents a guild channel that might be nested within a category channel.
    /// </summary>
    public interface ICategorizableGuildChannel : IGuildChannel
    {
        /// <summary>
        ///     Gets the ID of the category of this channel.
        ///     Returns <see langword="null"/> if the channel has no category.
        /// </summary>
        Snowflake? CategoryId { get; }
    }
}
