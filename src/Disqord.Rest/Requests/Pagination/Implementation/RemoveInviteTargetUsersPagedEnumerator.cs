using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Disqord.Rest;

public class RemoveInviteTargetUsersPagedEnumerator(
    IRestClient client,
    string code,
    Snowflake[] userIds,
    IRestRequestOptions? options,
    CancellationToken cancellationToken)
    : PagedEnumerator<Snowflake>(client, userIds.Length, options, cancellationToken)
{
    public override int PageSize => Discord.Limits.Rest.MaxInviteTargetUserAmount;

    private int _offset;

    protected override async Task<IReadOnlyList<Snowflake>> NextPageCoreAsync(
        IReadOnlyList<Snowflake>? previousPage, IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var amount = NextPageSize;
        var segment = new ArraySegment<Snowflake>(userIds, _offset, amount);
        _offset += amount;
        await Client.InternalRemoveInviteTargetUsersAsync(code, segment, options, cancellationToken).ConfigureAwait(false);

        return segment;
    }
}
