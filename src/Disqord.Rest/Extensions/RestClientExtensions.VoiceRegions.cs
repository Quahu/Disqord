using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Rest.Api;
using Qommon.Collections;
using Qommon.Collections.ReadOnly;

namespace Disqord.Rest
{
    public static partial class RestClientExtensions
    {
        public static async Task<IReadOnlyList<IVoiceRegion>> FetchVoiceRegionsAsync(this IRestClient client,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var models = await client.ApiClient.FetchVoiceRegionsAsync(options, cancellationToken).ConfigureAwait(false);
            return models.ToReadOnlyList(client, (x, client) => new TransientVoiceRegion(client, x));
        }
    }
}
