using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Qommon;

namespace Disqord.Rest;

public class FetchBansEnumerator : PagedEnumerator<IBan>
{
    public override int PageSize => Discord.Limits.Rest.FetchBansPageSize;

    private readonly Snowflake _guildId;
    private readonly FetchDirection _direction;
    private readonly Snowflake? _startFromId;

    public FetchBansEnumerator(
        IRestClient client,
        Snowflake guildId,
        int limit, FetchDirection direction, Snowflake? startFromId,
        IRestRequestOptions? options,
        CancellationToken cancellationToken)
        : base(client, limit, options, cancellationToken)
    {
        _guildId = guildId;
        _direction = direction;
        _startFromId = startFromId;
    }

    protected override Task<IReadOnlyList<IBan>> NextPageAsync(
        IReadOnlyList<IBan>? previousPage, IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var startFromId = _startFromId;
        if (previousPage != null && previousPage.Count > 0)
        {
            startFromId = _direction switch
            {
                FetchDirection.Before => previousPage[0].User.Id,
                FetchDirection.After => previousPage[^1].User.Id,
                _ => Throw.ArgumentOutOfRangeException<Snowflake>("direction"),
            };
        }

        return Client.InternalFetchBansAsync(_guildId, NextPageSize, _direction, startFromId, options, cancellationToken);
    }
}