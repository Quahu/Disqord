namespace Disqord
{
    /// <summary>
    ///     Represents a guild channel that might be nested within a category channel.
    /// </summary>
    public interface INestableChannel : IGuildChannel
    {
        /// <summary>
        ///     Gets the category ID of this channel.
        ///     Returns <see langword="null"/> if the channel is not bound to a category.
        /// </summary>
        Snowflake? CategoryId { get; }
    }
}
