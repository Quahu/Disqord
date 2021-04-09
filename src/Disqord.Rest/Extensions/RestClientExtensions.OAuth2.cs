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

        // TODO: fetch current authorization
    }
}
