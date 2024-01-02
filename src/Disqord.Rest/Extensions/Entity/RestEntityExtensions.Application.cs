using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Disqord.Rest;

public static partial class RestEntityExtensions
{
    public static Task<IReadOnlyList<ISku>> FetchSkusAsync(this IApplication application,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = application.GetRestClient();
        return client.FetchSkusAsync(application.Id, options, cancellationToken);
    }

    public static Task<IReadOnlyList<IEntitlement>> FetchEntitlementsAsync(this IApplication application,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = application.GetRestClient();
        return client.FetchEntitlementsAsync(application.Id, options, cancellationToken);
    }

    public static Task<IEntitlement> CreateTestEntitlementAsync(this IApplication application,
        Snowflake skuId, Snowflake ownerId, EntitlementOwnerType ownerType,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = application.GetRestClient();
        return client.CreateTestEntitlementAsync(application.Id, skuId, ownerId, ownerType, options, cancellationToken);
    }

    public static Task DeleteAsync(this IEntitlement entitlement,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = entitlement.GetRestClient();
        return client.DeleteTestEntitlementAsync(entitlement.ApplicationId, entitlement.Id, options, cancellationToken);
    }
}
