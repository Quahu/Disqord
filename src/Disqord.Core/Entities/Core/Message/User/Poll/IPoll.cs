using System;
using System.Collections.Generic;

namespace Disqord;

/// <summary>
///     Represents a poll.
/// </summary>
public interface IPoll : IEntity
{
    /// <summary>
    ///     Gets the question of this poll.
    /// </summary>
    IPollMedia Question { get; }

    /// <summary>
    ///     Gets the answers of this poll.
    /// </summary>
    IReadOnlyList<IPollAnswer> Answers { get; }

    /// <summary>
    ///     Gets the expiry of this poll.
    /// </summary>
    DateTimeOffset? Expiry { get; }

    /// <summary>
    ///     Gets whether this poll allows selection of multiple answers.
    /// </summary>
    bool AllowMultiselect { get; }

    /// <summary>
    ///     Gets the layout type of this poll.
    /// </summary>
    PollLayoutType LayoutType { get; }

    /// <summary>
    ///     Gets the results of this poll.
    /// </summary>
    IPollResults? Results { get; }
}
