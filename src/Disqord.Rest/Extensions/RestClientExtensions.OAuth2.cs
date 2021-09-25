using System;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Rest.Api;

namespace Disqord.Rest
{
    public static partial class RestClientExtensions
    {
        public static async Task<IApplication> FetchCurrentApplicationAsync(this IRestClient client, IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var model = await client.ApiClient.FetchCurrentApplicationAsync(options, cancellationToken).ConfigureAwait(false);
            return TransientApplication.Create(client, model);
        }

        // TODO: proper oauth abstractions
        public static async Task<IBearerAuthorization> FetchCurrentAuthorizationAsync(this IRestClient client, IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            if (client.ApiClient.Token is not BearerToken)
                throw new InvalidOperationException("This endpoint can only be used with a bearer token.");

            var model = await client.ApiClient.FetchCurrentAuthorizationAsync(options, cancellationToken).ConfigureAwait(false);
            return new TransientBearerAuthorization(client, model);
        }
    }
}
