namespace Disqord;

public interface ISelectionComponentOption
{
    /// <summary>
    ///     Gets the label of this selection component option.
    /// </summary>
    string Label { get; }

    /// <summary>
    ///     Gets the value of this selection component option.
    /// </summary>
    string Value { get; }

    /// <summary>
    ///     Gets the description of this selection component option.
    ///     Returns <see langword="null"/> if not set.
    /// </summary>
    string? Description { get; }

    /// <summary>
    ///     Gets the emoji of this selection component option.
    ///     Returns <see langword="null"/> if not set.
    /// </summary>
    IEmoji? Emoji { get; }

    /// <summary>
    ///     Gets whether this selection component option is selected by default.
    /// </summary>
    bool IsDefault { get; }
}
