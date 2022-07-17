namespace Disqord;

/// <summary>
///     Represents a guild channel that might be nested within a category channel.
/// </summary>
public interface ICategorizableGuildChannel : IGuildChannel
{
    /// <summary>
    ///     Gets the ID of the category channel of this channel.
    /// </summary>
    /// <returns>
    ///     The ID of the category channel or <see langword="null"/> if the channel has no category.
    /// </returns>
    Snowflake? CategoryId { get; }
}
