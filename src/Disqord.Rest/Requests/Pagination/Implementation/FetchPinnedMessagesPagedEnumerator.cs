using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Disqord.Rest;

public class FetchPinnedMessagesPagedEnumerator : PagedEnumerator<IRestPinnedUserMessage>
{
    public override int PageSize => Discord.Limits.Rest.FetchPinnedMessagesPageSize;

    private readonly Snowflake _channelId;
    private DateTimeOffset? _startFromDate;

    public FetchPinnedMessagesPagedEnumerator(
        IRestClient client,
        Snowflake channelId, int limit, DateTimeOffset? startFromDate,
        IRestRequestOptions? options,
        CancellationToken cancellationToken)
        : base(client, limit, options, cancellationToken)
    {
        _channelId = channelId;
        _startFromDate = startFromDate;
    }

    protected override async Task<IReadOnlyList<IRestPinnedUserMessage>> NextPageAsync(
        IReadOnlyList<IRestPinnedUserMessage>? previousPage, IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var response = await Client.InternalFetchPinnedMessagesAsync(_channelId, NextPageSize, _startFromDate, options, cancellationToken).ConfigureAwait(false);
        if (!response.HasMore)
        {
            RemainingCount = 0;
        }
        else
        {
            _startFromDate = response.LastPinnedAt;
        }

        return response.Messages;
    }
}
