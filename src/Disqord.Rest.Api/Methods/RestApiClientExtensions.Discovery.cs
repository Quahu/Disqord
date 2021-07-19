using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord.Rest.Api
{
    public static partial class RestApiClientExtensions
    {
        public static Task<GuildDiscoveryCategoryJsonModel[]> FetchDiscoveryCategoriesAsync(this IRestApiClient client, IRestRequestOptions options = null)
        {
            var route = Format(Route.Discovery.GetCategories);
            return client.ExecuteAsync<GuildDiscoveryCategoryJsonModel[]>(route, null, options);
        }

        public static Task<ValidateSearchTermJsonModel> ValidateDiscoverySearchTerm(this IRestApiClient client, string term, IRestRequestOptions options = null)
        {
            var queryParameters = new[] { new KeyValuePair<string, object>("term", term) };
            var route = Format(Route.Discovery.ValidateSearchTerm, queryParameters);

            return client.ExecuteAsync<ValidateSearchTermJsonModel>(route, null, options);
        }
    }
}