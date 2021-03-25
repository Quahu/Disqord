using System.Threading.Tasks;
using Disqord.Gateway;
using Disqord.Rest.Api;

namespace Disqord.Rest
{
    public static partial class RestClientExtensions
    {
        public static async Task<string> FetchGatewayUrlAsync(this IRestClient client, IRestRequestOptions options = null)
        {
            var model = await client.ApiClient.FetchGatewayAsync(options).ConfigureAwait(false);
            return model.Url;
        }

        public static async Task<IBotGatewayData> FetchBotGatewayDataAsync(this IRestClient client, IRestRequestOptions options = null)
        {
            var model = await client.ApiClient.FetchBotGatewayAsync(options).ConfigureAwait(false);
            return new TransientBotGatewayData(client, model);
        }
    }
}
