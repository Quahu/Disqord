using System.Threading;
using System.Threading.Tasks;
using Disqord.Gateway;
using Disqord.Rest.Api;

namespace Disqord.Rest;

public static partial class RestClientExtensions
{
    public static async Task<string> FetchGatewayUrlAsync(this IRestClient client,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var model = await client.ApiClient.FetchGatewayAsync(options, cancellationToken).ConfigureAwait(false);
        return model.Url;
    }

    public static async Task<IBotGatewayData> FetchBotGatewayDataAsync(this IRestClient client,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var model = await client.ApiClient.FetchBotGatewayAsync(options, cancellationToken).ConfigureAwait(false);
        return new TransientBotGatewayData(client, model);
    }
}