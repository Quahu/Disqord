using System.Threading.Tasks;
using Disqord.Serialization.Json;

namespace Disqord.Rest.Api
{
    public static partial class RestApiClientExtensions
    {
        public static Task<JsonModel> FetchCurrentApplicationAsync(this IRestApiClient client, IRestRequestOptions options = null)
        {
            var route = Format(Route.OAuth2.GetCurrentApplication);
            return client.ExecuteAsync<JsonModel>(route, null, options);
        }
    }
}
