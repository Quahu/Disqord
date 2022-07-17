using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Qommon;

namespace Disqord.Rest;

public class FetchGuildEventUsersPagedEnumerator : PagedEnumerator<IUser>
{
    public override int PageSize => Discord.Limits.Rest.FetchGuildEventUsersPageSize;

    private readonly Snowflake _guildId;
    private readonly Snowflake _eventId;
    private readonly FetchDirection _direction;
    private readonly Snowflake? _startFromId;
    private readonly bool? _withMember;

    public FetchGuildEventUsersPagedEnumerator(
        IRestClient client,
        Snowflake guildId, Snowflake eventId, int limit, FetchDirection direction, Snowflake? startFromId,
        bool? withMember,
        IRestRequestOptions? options,
        CancellationToken cancellationToken)
        : base(client, limit, options, cancellationToken)
    {
        _guildId = guildId;
        _eventId = eventId;
        _direction = direction;
        _startFromId = startFromId;
        _withMember = withMember;
    }

    protected override Task<IReadOnlyList<IUser>> NextPageAsync(
        IReadOnlyList<IUser>? previousPage, IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
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

        return Client.InternalFetchGuildEventUsersAsync(_guildId, _eventId, NextPageSize, _direction, startFromId, _withMember, options, cancellationToken);
    }
}