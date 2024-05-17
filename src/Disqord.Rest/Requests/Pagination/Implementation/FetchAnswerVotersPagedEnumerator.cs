using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Disqord.Rest;

public class FetchAnswerVotersPagedEnumerator : PagedEnumerator<IUser>
{
    public override int PageSize => Discord.Limits.Rest.FetchPollAnswerVotersPageSize;

    private readonly Snowflake _channelId;
    private readonly Snowflake _messageId;
    private readonly int _answerId;
    private readonly Snowflake? _startFromId;

    public FetchAnswerVotersPagedEnumerator(
        IRestClient client,
        Snowflake channelId, Snowflake messageId, int answerId, int limit, Snowflake? startFromId,
        IRestRequestOptions? options,
        CancellationToken cancellationToken)
        : base(client, limit, options, cancellationToken)
    {
        _channelId = channelId;
        _messageId = messageId;
        _answerId = answerId;
        _startFromId = startFromId;
    }

    protected override Task<IReadOnlyList<IUser>> NextPageAsync(
        IReadOnlyList<IUser>? previousPage, IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var startFromId = _startFromId;
        if (previousPage != null && previousPage.Count > 0)
            startFromId = previousPage[^1].Id;

        return Client.InternalFetchAnswerVotersAsync(_channelId, _messageId, _answerId, NextPageSize, startFromId, options, cancellationToken);
    }
}
