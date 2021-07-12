using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Disqord.Collections;
using Disqord.Rest.Api;
using Disqord.Serialization.Json;

namespace Disqord.Rest
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static partial class RestClientExtensions
    {
        public static async Task<IReadOnlyList<IVoiceRegion>> FetchVoiceRegionsAsync(this IRestClient client, IRestRequestOptions options = null)
        {
            var models = await client.ApiClient.FetchVoiceRegionsAsync(options).ConfigureAwait(false);
            return models.ToReadOnlyList(client, (x, client) => new TransientVoiceRegion(client, x));
        }
        
        //private static IRestApiClient GetApiClient(IDiscordEntity entity)
        //{
        //    if (entity.Client is not IRestDiscordClient client)
        //        throw new NotSupportedException("This entity's client does not support executing REST requests.");

        //    return client.ApiClient;
        //}
    }
}
