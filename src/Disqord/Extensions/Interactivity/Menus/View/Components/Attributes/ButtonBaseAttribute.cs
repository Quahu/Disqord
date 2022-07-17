namespace Disqord.Extensions.Interactivity.Menus;

/// <summary>
///     Represents the common properties used by <see cref="ButtonAttribute"/> and <see cref="LinkButtonAttribute"/>.
/// </summary>
public class ButtonBaseAttribute : ComponentAttribute
{
    public string? Label { get; init; }

    /// <summary>
    ///     Gets or sets the emoji <see cref="string"/> or <see cref="ulong"/> ID.
    ///     This can be a custom emoji (e.g. <c>"&lt;:professor:667582610431803437&gt;"</c> or <c>667582610431803437</c>) or a default Unicode emoji (e.g. <c>"🍿"</c>).
    ///     The type will be determined using <see cref="LocalCustomEmoji.TryParse(string, out LocalCustomEmoji)"/>, if the value is a <see cref="string"/>.
    /// </summary>
    public object? Emoji { get; init; }
}