namespace Disqord;

/// <summary>
///     Represents a pack sticker.
/// </summary>
public interface IPackSticker : ISticker
{
    /// <summary>
    ///     Gets the ID of the pack of this sticker.
    /// </summary>
    Snowflake PackId { get; }

    /// <summary>
    ///     Gets the position of this sticker in the pack.
    /// </summary>
    int Position { get; }
}