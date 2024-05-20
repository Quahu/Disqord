using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Rest.Api;
using Disqord.Rest.Pagination;
using Qommon;
using Qommon.Collections.ReadOnly;

namespace Disqord.Rest;

public static partial class RestClientExtensions
{
    public static IPagedEnumerable<IUser> EnumeratePollAnswerVoters(this IRestClient client,
        Snowflake channelId, Snowflake messageId, int answerId, int limit, Snowflake? startFromId = null,
        IRestRequestOptions? options = null)
    {
        Guard.IsGreaterThanOrEqualTo(limit, 0);

        return PagedEnumerable.Create((state, cancellationToken) =>
        {
            var (client, channelId, messageId, answerId, limit, startFromId, options) = state;
            return new FetchPollAnswerVotersPagedEnumerator(client, channelId, messageId, answerId, limit, startFromId, options, cancellationToken);
        }, (client, channelId, messageId, answerId, limit, startFromId, options));
    }

    public static Task<IReadOnlyList<IUser>> FetchPollAnswerVotersAsync(this IRestClient client,
        Snowflake channelId, Snowflake messageId, int answerId, int limit = Discord.Limits.Rest.FetchPollAnswerVotersPageSize, Snowflake? startFromId = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        if (limit == 0)
            return Task.FromResult(ReadOnlyList<IUser>.Empty);

        if (limit <= Discord.Limits.Rest.FetchPollAnswerVotersPageSize)
            return client.InternalFetchPollAnswerVotersAsync(channelId, messageId, answerId, limit, startFromId, options, cancellationToken);

        var enumerable = client.EnumeratePollAnswerVoters(channelId, messageId, answerId, limit, startFromId, options);
        return enumerable.FlattenAsync(cancellationToken);
    }

    internal static async Task<IReadOnlyList<IUser>> InternalFetchPollAnswerVotersAsync(this IRestClient client,
        Snowflake channelId, Snowflake messageId, int answerId, int limit, Snowflake? startFromId,
        IRestRequestOptions? options, CancellationToken cancellationToken)
    {
        var models = await client.ApiClient.FetchAnswerVotersAsync(channelId, messageId, answerId, limit, startFromId, options, cancellationToken).ConfigureAwait(false);
        return models.ToReadOnlyList(client, (x, client) => new TransientUser(client, x));
    }

    public static Task EndPollAsync(this IRestClient client,
        Snowflake channelId, Snowflake messageId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        return client.ApiClient.EndPollAsync(channelId, messageId, options, cancellationToken);
    }
}
