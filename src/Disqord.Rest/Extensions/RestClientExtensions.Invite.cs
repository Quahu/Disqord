using System.Threading;
using System.Threading.Tasks;
using Disqord.Rest.Api;

namespace Disqord.Rest;

public static partial class RestClientExtensions
{
    public static async Task<IInvite> FetchInviteAsync(this IRestClient client,
        string code, bool? withCounts = null, bool? withExpiration = null, Snowflake? eventId = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var model = await client.ApiClient.FetchInviteAsync(code, withCounts, withExpiration, eventId, options, cancellationToken).ConfigureAwait(false);
        return TransientInvite.Create(client, model);
    }

    public static Task DeleteInviteAsync(this IRestClient client,
        string code,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        return client.ApiClient.DeleteInviteAsync(code, options, cancellationToken);
    }
}