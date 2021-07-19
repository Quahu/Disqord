using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Collections;
using Disqord.Rest.Api;

namespace Disqord.Rest
{
    public static partial class RestClientExtensions
    {
        public static async Task<IReadOnlyList<IGuildDiscoveryCategory>> FetchDiscoveryCategoriesAsync(this IRestClient client, IRestRequestOptions options = null)
        {
            var models = await client.ApiClient.FetchDiscoveryCategoriesAsync(options);
            return models.ToReadOnlyList(client, static (x, client) => new TransientGuildDiscoveryCategory(client, x));
        }
    }
}