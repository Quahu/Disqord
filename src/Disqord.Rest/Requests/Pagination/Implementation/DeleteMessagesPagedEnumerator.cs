using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Disqord.Rest;

public class DeleteMessagesPagedEnumerator : PagedEnumerator<Snowflake>
{
    public override int PageSize => Discord.Limits.Rest.DeleteMessagesPageSize;

    private readonly Snowflake _channelId;
    private readonly Snowflake[] _messageIds;

    private int _offset;

    public DeleteMessagesPagedEnumerator(
        IRestClient client,
        Snowflake channelId, Snowflake[] messageIds,
        IRestRequestOptions? options,
        CancellationToken cancellationToken)
        : base(client, messageIds.Length, options, cancellationToken)
    {
        _channelId = channelId;
        _messageIds = messageIds;
    }

    protected override async Task<IReadOnlyList<Snowflake>> NextPageAsync(
        IReadOnlyList<Snowflake>? previousPage, IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var amount = NextPageSize;
        var segment = new ArraySegment<Snowflake>(_messageIds, _offset, amount);
        _offset += amount;
        await (amount == 1
            ? Client.DeleteMessageAsync(_channelId, segment[0], options, cancellationToken)
            : Client.InternalDeleteMessagesAsync(_channelId, segment, options, cancellationToken)).ConfigureAwait(false);

        return segment;
    }
}