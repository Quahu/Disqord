using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Qommon;

namespace Disqord.Rest;

public class FetchMessagesPagedEnumerator : PagedEnumerator<IMessage>
{
    public override int PageSize => Discord.Limits.Rest.FetchMessagesPageSize;

    private readonly Snowflake _guildId;
    private readonly FetchDirection _direction;
    private readonly Snowflake? _startFromId;

    public FetchMessagesPagedEnumerator(
        IRestClient client,
        Snowflake guildId, int limit, FetchDirection direction, Snowflake? startFromId,
        IRestRequestOptions? options,
        CancellationToken cancellationToken)
        : base(client, limit, options, cancellationToken)
    {
        _guildId = guildId;
        _direction = direction;
        _startFromId = startFromId;
    }

    protected override Task<IReadOnlyList<IMessage>> NextPageAsync(
        IReadOnlyList<IMessage>? previousPage, IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var startFromId = _startFromId;
        if (previousPage != null && previousPage.Count > 0)
        {
            startFromId = _direction switch
            {
                FetchDirection.Before => previousPage[^1].Id,
                FetchDirection.After => previousPage[0].Id,
                FetchDirection.Around => throw new NotImplementedException(),
                _ => Throw.ArgumentOutOfRangeException<Snowflake>("direction"),
            };
        }

        return Client.InternalFetchMessagesAsync(_guildId, NextPageSize, _direction, startFromId, options, cancellationToken);
    }
}