using System.Collections.Generic;
using System.Threading.Tasks;
using Qommon.Collections;
using Disqord.Rest.Api;

namespace Disqord.Rest
{
    public static partial class RestClientExtensions
    {
        public static async Task<IReadOnlyList<IVoiceRegion>> FetchVoiceRegionsAsync(this IRestClient client, IRestRequestOptions options = null)
        {
            var models = await client.ApiClient.FetchVoiceRegionsAsync(options).ConfigureAwait(false);
            return models.ToReadOnlyList(client, (x, client) => new TransientVoiceRegion(client, x));
        }
    }
}
