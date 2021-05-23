using System;
using System.Threading.Tasks;
using Disqord.Rest.Api;

namespace Disqord.Rest
{
    public static partial class RestClientExtensions
    {
        public static async Task<IApplication> FetchCurrentApplicationAsync(this IRestClient client, IRestRequestOptions options = null)
        {
            var model = await client.ApiClient.FetchCurrentApplicationAsync(options).ConfigureAwait(false);
            return TransientApplication.Create(client, model);
        }

        public static async Task<IBearerAuthorization> FetchCurrentAuthorizationAsync(this IRestClient client, IRestRequestOptions options = null)
        {
            if (client.ApiClient.Token is not BearerToken)
                throw new InvalidOperationException("This endpoint can only be used with a bearer token.");

            var model = await client.ApiClient.FetchCurrentAuthorizationAsync(options).ConfigureAwait(false);
            return new TransientBearerAuthorization(client, model);
        }
    }
}
