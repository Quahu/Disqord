using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord.Rest.Api
{
    public static partial class RestApiClientExtensions
    {
        public static Task<InviteJsonModel> FetchInviteAsync(this IRestApiClient client, string code, IRestRequestOptions options = null)
        {
            var route = Format(Route.Invite.GetInvite, code);
            return client.ExecuteAsync<InviteJsonModel>(route, null, options);
        }

        public static Task DeleteInviteAsync(this IRestApiClient client, string code, IRestRequestOptions options = null)
        {
            var route = Format(Route.Invite.DeleteInvite, code);
            return client.ExecuteAsync(route, null, options);
        }
    }
}
