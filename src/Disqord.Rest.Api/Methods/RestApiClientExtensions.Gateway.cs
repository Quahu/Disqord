using System.Threading.Tasks;
using Disqord.Serialization.Json;

namespace Disqord.Rest.Api
{
    public static partial class RestApiClientExtensions
    {
        public static Task<JsonModel> FetchGatewayAsync(this IRestApiClient client, IRestRequestOptions options = null)
        {
            var route = Format(Route.Gateway.GetGateway);
            return client.ExecuteAsync<JsonModel>(route, null, options);
        }

        public static Task<JsonModel> FetchBotGatewayAsync(this IRestApiClient client, IRestRequestOptions options = null)
        {
            var route = Format(Route.Gateway.GetBotGateway);
            return client.ExecuteAsync<JsonModel>(route, null, options);
        }
    }
}
