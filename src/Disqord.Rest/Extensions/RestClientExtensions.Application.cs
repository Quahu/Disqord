using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Rest.Api;
using Qommon.Collections.ReadOnly;

namespace Disqord.Rest;

public static partial class RestClientExtensions
{
    public static async Task<IReadOnlyList<ISku>> FetchSkusAsync(this IRestClient client,
        Snowflake applicationId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var models = await client.ApiClient.FetchSkusAsync(applicationId, options, cancellationToken).ConfigureAwait(false);
        return models.ToReadOnlyList(client, (model, client) => new TransientSku(client, model));
    }

    public static async Task<IReadOnlyList<IEntitlement>> FetchEntitlementsAsync(this IRestClient client,
        Snowflake applicationId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var models = await client.ApiClient.FetchEntitlementsAsync(applicationId, options, cancellationToken).ConfigureAwait(false);
        return models.ToReadOnlyList(client, (model, client) => new TransientEntitlement(client, model));
    }

    public static async Task<IEntitlement> CreateTestEntitlementAsync(this IRestClient client,
        Snowflake applicationId, Snowflake skuId, Snowflake ownerId, EntitlementOwnerType ownerType,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var content = new CreateEntitlementJsonRestRequestContent
        {
            SkuId = skuId,
            OwnerId = ownerId,
            OwnerType = ownerType
        };
        var model = await client.ApiClient.CreateTestEntitlementAsync(applicationId, content, options, cancellationToken).ConfigureAwait(false);
        return new TransientEntitlement(client, model);
    }

    public static Task DeleteTestEntitlementAsync(this IRestClient client,
        Snowflake applicationId, Snowflake entitlementId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        return client.ApiClient.DeleteTestEntitlementAsync(applicationId, entitlementId, options, cancellationToken);
    }
}
