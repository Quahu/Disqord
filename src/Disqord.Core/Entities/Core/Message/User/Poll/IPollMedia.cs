namespace Disqord;

/// <summary>
///     Represents poll media.
/// </summary>
public interface IPollMedia : IEntity
{
    /// <summary>
    ///     Gets the text of this poll media.
    /// </summary>
    string? Text { get; }

    /// <summary>
    ///     Gets the emoji of this poll media.
    /// </summary>
    IEmoji? Emoji { get; }
}
