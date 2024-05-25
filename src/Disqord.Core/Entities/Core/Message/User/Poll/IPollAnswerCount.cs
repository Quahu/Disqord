namespace Disqord;

/// <summary>
///     Represents the vote count of a poll answer.
/// </summary>
public interface IPollAnswerCount : IEntity
{
    /// <summary>
    ///     Gets the ID of the poll answer.
    /// </summary>
    int AnswerId { get; }

    /// <summary>
    ///     Gets the amount of users that selected the poll answer.
    /// </summary>
    int Count { get; }

    /// <summary>
    ///     Gets whether the bot has selected the poll answer.
    /// </summary>
    bool HasOwnVote { get; }
}
