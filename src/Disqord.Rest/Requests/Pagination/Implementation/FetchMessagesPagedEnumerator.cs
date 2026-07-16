using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Qommon;

namespace Disqord.Rest;

public class FetchMessagesPagedEnumerator : PagedEnumerator<IMessage>
{
    public override int PageSize => Discord.Limits.Rest.FetchMessagesPageSize;

    private readonly Snowflake _channelId;
    private readonly FetchDirection _direction;
    private readonly Snowflake? _startFromId;
    private bool _isFirstPage = true;

    public FetchMessagesPagedEnumerator(
        IRestClient client,
        Snowflake channelId, int limit, FetchDirection direction, Snowflake? startFromId,
        IRestRequestOptions? options,
        CancellationToken cancellationToken)
        : base(client, limit, options, cancellationToken)
    {
        _channelId = channelId;
        _direction = direction;
        _startFromId = startFromId;
    }

    protected override Task<IReadOnlyList<IMessage>> NextPageCoreAsync(
        IReadOnlyList<IMessage>? previousPage, IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var startFromId = _startFromId;
        var direction = _direction;

        if (previousPage != null && previousPage.Count > 0)
        {
            if (_direction == FetchDirection.Around)
            {
                if (_isFirstPage)
                {
                    startFromId = previousPage[^1].Id;
                    direction = FetchDirection.After;
                }
                else
                {
                    startFromId = previousPage[^1].Id;
                    direction = FetchDirection.After;
                }
            }
            else
            {
                startFromId = _direction switch
                {
                    FetchDirection.Before => previousPage[^1].Id,
                    FetchDirection.After => previousPage[0].Id,
                    _ => Throw.ArgumentOutOfRangeException<Snowflake>("direction"),
                };
            }
        }

        if (_isFirstPage)
            _isFirstPage = false;

        return Client.InternalFetchMessagesAsync(_channelId, NextPageSize, direction, startFromId, options, cancellationToken);
    }
}
