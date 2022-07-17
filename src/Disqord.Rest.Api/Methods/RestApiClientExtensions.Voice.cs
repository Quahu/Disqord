using System.Threading;
using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord.Rest.Api;

public static partial class RestApiClientExtensions
{
    public static Task<VoiceRegionJsonModel[]> FetchVoiceRegionsAsync(this IRestApiClient client,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Voice.GetVoiceRegions);
        return client.ExecuteAsync<VoiceRegionJsonModel[]>(route, null, options, cancellationToken);
    }
}