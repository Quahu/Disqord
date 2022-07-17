using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Disqord.Rest;

public class FetchMembersPagedEnumerator : PagedEnumerator<IMember>
{
    public override int PageSize => Discord.Limits.Rest.FetchMembersPageSize;

    private readonly Snowflake _guildId;
    private readonly Snowflake? _startFromId;

    public FetchMembersPagedEnumerator(
        IRestClient client,
        Snowflake guildId, int limit, Snowflake? startFromId,
        IRestRequestOptions? options,
        CancellationToken cancellationToken)
        : base(client, limit, options, cancellationToken)
    {
        _guildId = guildId;
        _startFromId = startFromId;
    }

    protected override Task<IReadOnlyList<IMember>> NextPageAsync(
        IReadOnlyList<IMember>? previousPage, IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var startFromId = _startFromId;
        if (previousPage != null && previousPage.Count > 0)
            startFromId = previousPage[^1].Id;

        return Client.InternalFetchMembersAsync(_guildId, NextPageSize, startFromId, options, cancellationToken);
    }
}