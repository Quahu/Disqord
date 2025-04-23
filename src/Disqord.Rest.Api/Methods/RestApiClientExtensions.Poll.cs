using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Models;
using Qommon;

namespace Disqord.Rest.Api;

public static partial class RestApiClientExtensions
{
    public static Task<UserJsonModel[]> FetchAnswerVotersAsync(this IRestApiClient client,
        Snowflake channelId, Snowflake messageId, int answerId,
        int limit = Discord.Limits.Rest.FetchPollAnswerVotersPageSize, Snowflake? startFromId = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        Guard.IsBetweenOrEqualTo(limit, 0, Discord.Limits.Rest.FetchPollAnswerVotersPageSize);

        var queryParameters = new Dictionary<string, object>(startFromId != null ? 2 : 1)
        {
            ["limit"] = limit
        };

        if (startFromId != null)
            queryParameters["after"] = startFromId;

        var route = Format(Route.Poll.GetAnswerVoters, queryParameters, channelId, messageId, answerId);
        return client.ExecuteAsync<UserJsonModel[]>(route, null, options, cancellationToken);
    }

    public static Task EndPollAsync(this IRestApiClient client,
        Snowflake channelId, Snowflake messageId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Poll.EndPoll, channelId, messageId);
        return client.ExecuteAsync(route, null, options, cancellationToken);
    }
}
