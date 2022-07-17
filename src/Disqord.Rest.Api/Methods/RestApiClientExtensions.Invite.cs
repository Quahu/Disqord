using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord.Rest.Api;

public static partial class RestApiClientExtensions
{
    public static Task<InviteJsonModel> FetchInviteAsync(this IRestApiClient client,
        string code, bool? withCounts = null, bool? withExpiration = null, Snowflake? eventId = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        IFormattedRoute route;
        if (withCounts != null || withExpiration != null || eventId != null)
        {
            var queryParameters = new Dictionary<string, object>();

            if (withCounts != null)
                queryParameters["with_counts"] = withCounts.Value;

            if (withExpiration != null)
                queryParameters["with_expiration"] = withExpiration.Value;

            if (eventId != null)
                queryParameters["guild_scheduled_event_id"] = eventId.Value;

            route = Format(Route.Invite.GetInvite, queryParameters, code);
        }
        else
        {
            route = Format(Route.Invite.GetInvite, code);
        }

        return client.ExecuteAsync<InviteJsonModel>(route, null, options, cancellationToken);
    }

    public static Task DeleteInviteAsync(this IRestApiClient client,
        string code,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Invite.DeleteInvite, code);
        return client.ExecuteAsync(route, null, options, cancellationToken);
    }
}
