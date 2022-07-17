namespace Disqord;

/// <summary>
///     Represents a Discord application that is sold on the game store.
/// </summary>
public interface IStoreApplication : IApplication
{
    /// <summary>
    ///     Gets the ID of the linked guild of this application.
    /// </summary>
    Snowflake? GuildId { get; }

    /// <summary>
    ///     Gets the ID of the "Game SKU", if it exists, of this application.
    /// </summary>
    Snowflake? PrimarySkuId { get; }

    /// <summary>
    ///     Gets the slug that links to the store page of this application.
    /// </summary>
    string? Slug { get; }

    /// <summary>
    ///     Gets the cover image hash visible on store embeds of this application.
    /// </summary>
    string? CoverHash { get; }
}
