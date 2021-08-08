using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord.Rest.Api
{
    public static partial class RestApiClientExtensions
    {
        public static Task<InviteJsonModel> FetchInviteAsync(this IRestApiClient client, string code, bool? withCounts = null, bool? withExpiration = null, IRestRequestOptions options = null)
        {
            FormattedRoute route;

            if (withCounts != null || withExpiration != null)
            {
                var queryParameters = new Dictionary<string, object>(withCounts != null && withExpiration != null ? 2 : 1);

                if (withCounts != null)
                    queryParameters["with_counts"] = withCounts.Value;

                if (withExpiration != null)
                    queryParameters["with_expiration"] = withExpiration.Value;

                route = Format(Route.Invite.GetInvite, queryParameters, code);
            }
            else
            {
                route = Format(Route.Invite.GetInvite, code);
            }

            return client.ExecuteAsync<InviteJsonModel>(route, null, options);
        }

        public static Task DeleteInviteAsync(this IRestApiClient client, string code, IRestRequestOptions options = null)
        {
            var route = Format(Route.Invite.DeleteInvite, code);
            return client.ExecuteAsync(route, null, options);
        }
    }
}
