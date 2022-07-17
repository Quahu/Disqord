namespace Disqord.Gateway;

/// <summary>
///     Represents a member's custom activity, i.e. the "custom status" settable in the desktop client.
/// </summary>
public interface ICustomActivity : IActivity
{
    /// <summary>
    ///     Gets the text of this custom activity.
    /// </summary>
    string? Text { get; }

    /// <summary>
    ///     Gets the emoji of this custom activity.
    ///     Returns <see langword="null"/> if the member did not set one.
    /// </summary>
    IEmoji? Emoji { get; }
}