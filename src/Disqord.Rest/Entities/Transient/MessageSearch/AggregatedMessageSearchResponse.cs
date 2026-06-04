using System.Collections.Generic;

namespace Disqord.Rest;

internal sealed class AggregatedMessageSearchResponse(
    int totalResultCount,
    IReadOnlyCollection<IMessageSearchFoundMessage> foundMessages,
    IReadOnlyCollection<IChannel> threads,
    bool isDoingDeepHistoricalIndex,
    int? documentsIndexed)
    : IMessageSearchResponse
{
    /// <inheritdoc/>
    public int TotalResultCount { get; } = totalResultCount;

    /// <inheritdoc/>
    public IReadOnlyCollection<IMessageSearchFoundMessage> FoundMessages { get; } = foundMessages;

    /// <inheritdoc/>
    public IReadOnlyCollection<IChannel> Threads { get; } = threads;

    /// <inheritdoc/>
    public bool IsDoingDeepHistoricalIndex { get; } = isDoingDeepHistoricalIndex;

    /// <inheritdoc/>
    public int? DocumentsIndexed { get; } = documentsIndexed;
}
