using System.Threading;
using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord.Rest.Api;

public static partial class RestApiClientExtensions
{
    public static Task<ApplicationJsonModel> FetchCurrentApplicationAsync(this IRestApiClient client,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.OAuth2.GetCurrentApplication);
        return client.ExecuteAsync<ApplicationJsonModel>(route, null, options, cancellationToken);
    }

    public static Task<AuthorizationJsonModel> FetchCurrentAuthorizationAsync(this IRestApiClient client,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.OAuth2.GetCurrentAuthorization);
        return client.ExecuteAsync<AuthorizationJsonModel>(route, null, options, cancellationToken);
    }
}