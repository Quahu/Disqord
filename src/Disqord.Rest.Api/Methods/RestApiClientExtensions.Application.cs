using System.Threading;
using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord.Rest.Api;

public static partial class RestApiClientExtensions
{
    public static Task<SkuJsonModel[]> FetchSkusAsync(this IRestApiClient client,
        Snowflake applicationId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Montetization.GetSkus, applicationId);
        return client.ExecuteAsync<SkuJsonModel[]>(route, null, options, cancellationToken);
    }

    public static Task<EntitlementJsonModel[]> FetchEntitlementsAsync(this IRestApiClient client,
        Snowflake applicationId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Montetization.GetEntitlements, applicationId);
        return client.ExecuteAsync<EntitlementJsonModel[]>(route, null, options, cancellationToken);
    }

    public static Task<EntitlementJsonModel> CreateTestEntitlementAsync(this IRestApiClient client,
        Snowflake applicationId, CreateEntitlementJsonRestRequestContent content,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Montetization.CreateTestEntitlement, applicationId);
        return client.ExecuteAsync<EntitlementJsonModel>(route, content, options, cancellationToken);
    }

    public static Task<EntitlementJsonModel> DeleteTestEntitlementAsync(this IRestApiClient client,
        Snowflake applicationId, Snowflake entitlementId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Montetization.DeleteTestEntitlement, applicationId, entitlementId);
        return client.ExecuteAsync<EntitlementJsonModel>(route, null, options, cancellationToken);
    }
}
