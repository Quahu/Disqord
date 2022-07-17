using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Disqord.Rest;

public class FetchReactionsPagedEnumerator : PagedEnumerator<IUser>
{
    public override int PageSize => Discord.Limits.Rest.FetchReactionsPageSize;

    private readonly Snowflake _channelId;
    private readonly Snowflake _messageId;
    private readonly LocalEmoji _emoji;
    private readonly Snowflake? _startFromId;

    public FetchReactionsPagedEnumerator(
        IRestClient client,
        Snowflake channelId, Snowflake messageId, LocalEmoji emoji, int limit, Snowflake? startFromId,
        IRestRequestOptions? options,
        CancellationToken cancellationToken)
        : base(client, limit, options, cancellationToken)
    {
        _channelId = channelId;
        _messageId = messageId;
        _emoji = emoji;
        _startFromId = startFromId;
    }

    protected override Task<IReadOnlyList<IUser>> NextPageAsync(
        IReadOnlyList<IUser>? previousPage, IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var startFromId = _startFromId;
        if (previousPage != null && previousPage.Count > 0)
            startFromId = previousPage[^1].Id;

        return Client.InternalFetchReactionsAsync(_channelId, _messageId, _emoji, NextPageSize, startFromId, options, cancellationToken);
    }
}