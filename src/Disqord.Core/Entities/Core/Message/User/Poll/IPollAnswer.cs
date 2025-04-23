namespace Disqord;

/// <summary>
///     Represents a poll answer.
/// </summary>
public interface IPollAnswer : IEntity
{
    /// <summary>
    ///     Gets the ID of the poll answer. This is used to match <see cref="IPollAnswerCount"/> based on the ID.
    /// </summary>
    int Id { get; }

    /// <summary>
    ///     Gets the media of this poll answer.
    /// </summary>
    IPollMedia Media { get; }
}
