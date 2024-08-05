using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Qommon.Collections.ReadOnly;

namespace Disqord.Rest;

public class CreateBansPagedEnumerator : PagedEnumerator<IBulkBanResponse>
{
    public override int PageSize => Discord.Limits.Guild.MaxBulkBanUsersAmount;

    private readonly Snowflake _guildId;
    private readonly Snowflake[] _userIds;
    private readonly string? _reason;
    private readonly TimeSpan? _deleteMessagesTime = null;

    private int _offset;

    public CreateBansPagedEnumerator(
        IRestClient client,
        Snowflake guildId, Snowflake[] userIds, string? reason = null, TimeSpan? deleteMessagesTime = null,
        IRestRequestOptions? options = null,
        CancellationToken cancellationToken = default)
        : base(client, userIds.Length, options, cancellationToken)
    {
        _guildId = guildId;
        _userIds = userIds;
        _reason = reason;
        _deleteMessagesTime = deleteMessagesTime;
    }

    protected override async Task<IReadOnlyList<IBulkBanResponse>> NextPageAsync(IReadOnlyList<IBulkBanResponse>? previousPage, IRestRequestOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var amount = NextPageSize;
        var segment = new ArraySegment<Snowflake>(_userIds, _offset, amount);
        _offset += amount;
        var response = await Client.InternalCreateBansAsync(_guildId, segment, _reason, _deleteMessagesTime, options,
            cancellationToken);
        return Enumerable.Repeat(response, PageSize).ToReadOnlyList();
    }
}
