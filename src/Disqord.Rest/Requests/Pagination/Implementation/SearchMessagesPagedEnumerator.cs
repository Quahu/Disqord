using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Disqord.Rest;

public class SearchMessagesPagedEnumerator : PagedEnumerator<IMessageSearchResponse, IMessageSearchFoundMessage>
{
    public override int PageSize => Discord.Limits.Rest.SearchMessagesPageSize;

    /// <summary>
    ///     Gets the current page's full search response.
    /// </summary>
    public new IMessageSearchResponse? CurrentPage => base.CurrentPage;

    private readonly Snowflake _guildId;
    private readonly LocalMessageSearch _search;
    private readonly MessageSearchSortMode _sortBy;
    private readonly MessageSearchSortOrder _sortOrder;
    private readonly Snowflake? _afterId;
    private readonly Snowflake? _beforeId;
    private readonly bool _waitUntilIndexReady;

    private int _offset;

    public SearchMessagesPagedEnumerator(
        IRestClient client,
        Snowflake guildId,
        LocalMessageSearch search,
        int limit,
        MessageSearchSortMode sortBy,
        MessageSearchSortOrder sortOrder,
        Snowflake? afterId,
        Snowflake? beforeId,
        bool waitUntilIndexReady,
        IRestRequestOptions? options,
        CancellationToken cancellationToken)
        : base(client, limit, options, cancellationToken)
    {
        _guildId = guildId;
        _search = search;
        _sortBy = sortBy;
        _sortOrder = sortOrder;
        _afterId = afterId;
        _beforeId = beforeId;
        _waitUntilIndexReady = waitUntilIndexReady;
    }

    protected override async Task<IMessageSearchResponse?> NextPageAsync(IMessageSearchResponse? previousPage,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        if (_offset > Discord.Limits.Rest.MaxSearchMessagesOffset)
            return null;

        var response = await Client.InternalSearchMessagesAsync(_guildId, _search, NextPageSize, _sortBy, _sortOrder,
            offset: _offset, minId: _afterId, maxId: _beforeId,
            _waitUntilIndexReady, options, cancellationToken).ConfigureAwait(false);

        _offset += response.FoundMessages.Count;
        return response;
    }

    protected override IReadOnlyList<IMessageSearchFoundMessage> GetPageItems(IMessageSearchResponse page)
        => page.FoundMessages as IReadOnlyList<IMessageSearchFoundMessage> ?? page.FoundMessages.ToArray();

    protected override int GetConsumedCount(IMessageSearchResponse page)
        => page.FoundMessages.Count;
}
