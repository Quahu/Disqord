using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Disqord.Rest;

public class FetchThreadMembersPagedEnumerator : PagedEnumerator<IRestThreadMember>
{
    public override int PageSize => Discord.Limits.Rest.FetchThreadMembersPageSize;

    private readonly Snowflake _threadId;
    private readonly Snowflake? _startFromId;
    private readonly bool _withMember;

    public FetchThreadMembersPagedEnumerator(
        IRestClient client,
        Snowflake threadId, int limit, Snowflake? startFromId,
        bool withMember,
        IRestRequestOptions? options,
        CancellationToken cancellationToken)
        : base(client, limit, options, cancellationToken)
    {
        _threadId = threadId;
        _startFromId = startFromId;
        _withMember = withMember;
    }

    protected override Task<IReadOnlyList<IRestThreadMember>> NextPageAsync(
        IReadOnlyList<IRestThreadMember>? previousPage, IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var startFromId = _startFromId;
        if (previousPage != null && previousPage.Count > 0)
            startFromId = previousPage[^1].Id;

        return Client.InternalFetchThreadMembersAsync(_threadId, NextPageSize, startFromId, _withMember, options, cancellationToken);
    }
}
