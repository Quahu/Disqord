using System.Threading;
using System.Threading.Tasks;
using Disqord.Rest.Api;

namespace Disqord.Rest;

public static partial class RestClientExtensions
{
    public static async Task<IApplication> FetchCurrentApplicationAsync(this IRestClient client,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var model = await client.ApiClient.FetchCurrentApplicationAsync(options, cancellationToken).ConfigureAwait(false);
        return TransientApplication.Create(client, model);
    }
}