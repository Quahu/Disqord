using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Models;
using Qommon;

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
        Snowflake applicationId, int limit = Discord.Limits.Rest.FetchEntitlementsPageSize,
        Snowflake? userId = null, Snowflake[]? skuIds = null,
        Snowflake? beforeId = null, Snowflake? afterId = null,
        Snowflake? guildId = null, bool excludeEnded = false,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        Guard.IsBetweenOrEqualTo(limit, 1, Discord.Limits.Rest.FetchEntitlementsPageSize);

        var queryParameters = new Dictionary<string, object>(7)
        {
            ["limit"] = limit
        };

        if (userId != null)
            queryParameters["user_id"] = userId;

        if (skuIds != null)
            queryParameters["sku_ids"] = skuIds;

        if (beforeId != null)
            queryParameters["before"] = beforeId;

        if (afterId != null)
            queryParameters["after"] = afterId;

        if (guildId != null)
            queryParameters["guild_id"] = guildId;

        if (excludeEnded)
            queryParameters["exclude_ended"] = excludeEnded;

        var route = Format(Route.Montetization.GetEntitlements, queryParameters, applicationId);
        return client.ExecuteAsync<EntitlementJsonModel[]>(route, null, options, cancellationToken);
    }

    public static Task ConsumeEntitlementAsync(this IRestApiClient client,
        Snowflake applicationId, Snowflake entitlementId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Montetization.ConsumeEntitlement, applicationId, entitlementId);
        return client.ExecuteAsync(route, null, options, cancellationToken);
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
