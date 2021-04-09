using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord.Rest.Api
{
    public static partial class RestApiClientExtensions
    {
        public static Task<ApplicationJsonModel> FetchCurrentApplicationAsync(this IRestApiClient client, IRestRequestOptions options = null)
        {
            var route = Format(Route.OAuth2.GetCurrentApplication);
            return client.ExecuteAsync<ApplicationJsonModel>(route, null, options);
        }
        
        // TODO: fetch current authorization
    }
}
