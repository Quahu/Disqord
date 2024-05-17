using System.Collections.Generic;

namespace Disqord;

/// <summary>
///     Represents the results of a poll.
/// </summary>
public interface IPollResults : IEntity
{
    /// <summary>
    ///     Gets whether the votes have been precisely counted.
    /// </summary>
    bool IsFinalized { get; }

    /// <summary>
    ///     Gets the vote counts of each answer.
    /// </summary>
    IReadOnlyList<IPollAnswerCount> AnswerCounts { get; }
}
