using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Disqord.Rest;

public class FetchJoinedPrivateArchivedThreadsPagedEnumerator : PagedEnumerator<IThreadChannel>
{
    public override int PageSize => Discord.Limits.Rest.FetchThreadsPageSize;

    private readonly Snowflake _channelId;
    private readonly Snowflake? _startFromId;

    public FetchJoinedPrivateArchivedThreadsPagedEnumerator(
        IRestClient client,
        Snowflake channelId, int limit, Snowflake? startFromId,
        IRestRequestOptions? options,
        CancellationToken cancellationToken)
        : base(client, limit, options, cancellationToken)
    {
        _channelId = channelId;
        _startFromId = startFromId;
    }

    protected override async Task<IReadOnlyList<IThreadChannel>> NextPageAsync(
        IReadOnlyList<IThreadChannel>? previousPage, IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var startFromId = _startFromId;
        if (previousPage != null && previousPage.Count > 0)
            startFromId = previousPage[^1].Id;

        var response = await Client.InternalFetchJoinedPrivateArchivedThreadsAsync(_channelId, NextPageSize, startFromId, options, cancellationToken).ConfigureAwait(false);
        if (!response.HasMore)
            RemainingCount = 0;

        return response.Threads;
    }
}