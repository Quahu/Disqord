using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Disqord.Rest;

public class FetchArchivedThreadsPagedEnumerator : PagedEnumerator<IThreadChannel>
{
    public override int PageSize => Discord.Limits.Rest.FetchThreadsPageSize;

    private readonly Snowflake _channelId;
    private readonly DateTimeOffset? _startFromTime;
    private readonly bool _enumeratePublicThreads;

    public FetchArchivedThreadsPagedEnumerator(
        IRestClient client,
        Snowflake channelId, int limit, DateTimeOffset? startFromTime, bool enumeratePublicThreads,
        IRestRequestOptions? options,
        CancellationToken cancellationToken)
        : base(client, limit, options, cancellationToken)
    {
        _channelId = channelId;
        _startFromTime = startFromTime;
        _enumeratePublicThreads = enumeratePublicThreads;
    }

    protected override async Task<IReadOnlyList<IThreadChannel>> NextPageAsync(
        IReadOnlyList<IThreadChannel>? previousPage, IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var startFromTime = _startFromTime;
        if (previousPage != null && previousPage.Count > 0)
            startFromTime = previousPage[^1].Metadata.ArchiveStateChangedAt;

        var task = _enumeratePublicThreads
            ? Client.InternalFetchPublicArchivedThreadsAsync(_channelId, NextPageSize, startFromTime, options, cancellationToken)
            : Client.InternalFetchPrivateArchivedThreadsAsync(_channelId, NextPageSize, startFromTime, options, cancellationToken);

        var response = await task.ConfigureAwait(false);
        if (!response.HasMore)
            RemainingCount = 0;

        return response.Threads;
    }
}