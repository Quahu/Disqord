using System.Collections.Generic;

namespace Disqord.Rest;

/// <summary>
///     Represents the response of a guild message search.
/// </summary>
public interface IMessageSearchResponse
{
    /// <summary>
    ///     Gets the total number of results matching the search criteria.
    /// </summary>
    int TotalResultCount { get; }

    /// <summary>
    ///     Gets the messages that matched the search criteria (the hits).
    /// </summary>
    IReadOnlyCollection<IMessageSearchFoundMessage> FoundMessages { get; }

    /// <summary>
    ///     Gets the threads referenced by the search results.
    /// </summary>
    IReadOnlyCollection<IChannel> Threads { get; }

    /// <summary>
    ///     Gets whether a deep historical index is in progress.
    /// </summary>
    bool IsDoingDeepHistoricalIndex { get; }

    /// <summary>
    ///     Gets the number of documents indexed, if available.
    /// </summary>
    int? DocumentsIndexed { get; }
}
