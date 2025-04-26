using System;
using System.Collections.Generic;
using System.IO;
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
        int limit = Discord.Limits.Rest.FetchEntitlementsPageSize,
        Snowflake? userId = null, IEnumerable<Snowflake>? skuIds = null,
        Snowflake? beforeId = null, Snowflake? afterId = null,
        Snowflake? guildId = null, bool excludeEnded = false,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = application.GetRestClient();
        return client.FetchEntitlementsAsync(application.Id, limit, userId, skuIds, beforeId, afterId, guildId, excludeEnded, options, cancellationToken);
    }

    public static Task ConsumeEntitlementAsync(this IApplication application,
        Snowflake entitlementId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = application.GetRestClient();
        return client.ConsumeEntitlementAsync(application.Id, entitlementId, options, cancellationToken);
    }

    public static Task<IEntitlement> CreateTestEntitlementAsync(this IApplication application,
        Snowflake skuId, Snowflake ownerId, EntitlementOwnerType ownerType,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = application.GetRestClient();
        return client.CreateTestEntitlementAsync(application.Id, skuId, ownerId, ownerType, options, cancellationToken);
    }

    public static Task DeleteTestEntitlementAsync(this IApplication application,
        Snowflake entitlementId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = application.GetRestClient();
        return client.DeleteTestEntitlementAsync(application.Id, entitlementId, options, cancellationToken);
    }

    public static Task ConsumeAsync(this IEntitlement entitlement,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = entitlement.GetRestClient();
        return client.ConsumeEntitlementAsync(entitlement.ApplicationId, entitlement.Id, options, cancellationToken);
    }

    public static Task DeleteAsync(this IEntitlement entitlement,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = entitlement.GetRestClient();
        return client.DeleteTestEntitlementAsync(entitlement.ApplicationId, entitlement.Id, options, cancellationToken);
    }

    public static Task<IReadOnlyList<IApplicationEmoji>> FetchEmojisAsync(this IApplication application,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = application.GetRestClient();
        return client.FetchApplicationEmojisAsync(application.Id, options, cancellationToken);
    }

    public static Task<IApplicationEmoji> FetchEmojiAsync(this IApplication application,
        Snowflake emojiId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = application.GetRestClient();
        return client.FetchApplicationEmojiAsync(application.Id, emojiId, options, cancellationToken);
    }

    public static Task<IApplicationEmoji> CreateEmojiAsync(this IApplication application,
        string name, Stream image,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = application.GetRestClient();
        return client.CreateApplicationEmojiAsync(application.Id, name, image, options, cancellationToken);
    }

    public static Task<IApplicationEmoji> ModifyEmojiAsync(this IApplication application,
        Snowflake emojiId, Action<ModifyApplicationEmojiActionProperties> action,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = application.GetRestClient();
        return client.ModifyApplicationEmojiAsync(application.Id, emojiId, action, options, cancellationToken);
    }

    public static Task DeleteEmojiAsync(this IApplication application,
        Snowflake emojiId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = application.GetRestClient();
        return client.DeleteApplicationEmojiAsync(application.Id, emojiId, options, cancellationToken);
    }
}
