using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Qommon;

namespace Disqord.Rest;

public class FetchGuildsPagedEnumerator : PagedEnumerator<IPartialGuild>
{
    public override int PageSize => Discord.Limits.Rest.FetchGuildsPageSize;

    private readonly FetchDirection _direction;
    private readonly Snowflake? _startFromId;

    public FetchGuildsPagedEnumerator(
        IRestClient client,
        int limit, FetchDirection direction, Snowflake? startFromId,
        IRestRequestOptions? options,
        CancellationToken cancellationToken)
        : base(client, limit, options, cancellationToken)
    {
        _direction = direction;
        _startFromId = startFromId;
    }

    protected override Task<IReadOnlyList<IPartialGuild>> NextPageAsync(
        IReadOnlyList<IPartialGuild>? previousPage, IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var startFromId = _startFromId;
        if (previousPage != null && previousPage.Count > 0)
        {
            startFromId = _direction switch
            {
                FetchDirection.Before => previousPage[0].Id,
                FetchDirection.After => previousPage[^1].Id,
                _ => Throw.ArgumentOutOfRangeException<Snowflake>("direction"),
            };
        }

        return Client.InternalFetchGuildsAsync(NextPageSize, _direction, startFromId, options, cancellationToken);
    }
}
