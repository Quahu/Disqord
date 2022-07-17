using System.Threading;
using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord.Rest.Api;

public static partial class RestApiClientExtensions
{
    public static Task<GatewayJsonModel> FetchGatewayAsync(this IRestApiClient client,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Gateway.GetGateway);
        return client.ExecuteAsync<GatewayJsonModel>(route, null, options, cancellationToken);
    }

    public static Task<BotGatewayJsonModel> FetchBotGatewayAsync(this IRestApiClient client,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Gateway.GetBotGateway);
        return client.ExecuteAsync<BotGatewayJsonModel>(route, null, options, cancellationToken);
    }
}