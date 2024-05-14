using System.Collections.Generic;
using System.Linq;
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
        Snowflake applicationId, int limit = Discord.Limits.Rest.FetchEntitlementsPageSize,
        Snowflake? userId = null, IEnumerable<Snowflake>? skuIds = null,
        Snowflake? beforeId = null, Snowflake? afterId = null,
        Snowflake? guildId = null, bool excludeEnded = false,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var models = await client.ApiClient.FetchEntitlementsAsync(applicationId, limit, userId, skuIds?.ToArray(), beforeId, afterId, guildId, excludeEnded, options, cancellationToken).ConfigureAwait(false);
        return models.ToReadOnlyList(client, (model, client) => new TransientEntitlement(client, model));
    }

    public static Task ConsumeEntitlementAsync(this IRestClient client,
        Snowflake applicationId, Snowflake entitlementId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        return client.ApiClient.ConsumeEntitlementAsync(applicationId, entitlementId, options, cancellationToken);
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
