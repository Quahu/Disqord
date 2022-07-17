using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Rest.Api;
using Disqord.Rest.Pagination;
using Qommon;
using Qommon.Collections.ReadOnly;

namespace Disqord.Rest;

public static partial class RestClientExtensions
{
    public static async Task<ICurrentUser> FetchCurrentUserAsync(this IRestClient client,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var model = await client.ApiClient.FetchCurrentUserAsync(options, cancellationToken).ConfigureAwait(false);
        return new TransientCurrentUser(client, model);
    }

    public static async Task<IRestUser?> FetchUserAsync(this IRestClient client,
        Snowflake userId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var model = await client.ApiClient.FetchUserAsync(userId, options, cancellationToken).ConfigureAwait(false);
            return new TransientRestUser(client, model);
        }
        catch (RestApiException ex) when (ex.IsError(RestApiErrorCode.UnknownUser))
        {
            return null;
        }
    }

    public static async Task<ICurrentUser> ModifyCurrentUserAsync(this IRestClient client,
        Action<ModifyCurrentUserActionProperties> action,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(action);

        var properties = new ModifyCurrentUserActionProperties();
        action(properties);
        var content = new ModifyCurrentUserJsonRestRequestContent
        {
            Username = properties.Name,
            Avatar = properties.Avatar
        };

        var model = await client.ApiClient.ModifyCurrentUserAsync(content, options, cancellationToken).ConfigureAwait(false);
        return new TransientCurrentUser(client, model);
    }

    public static IPagedEnumerable<IPartialGuild> EnumerateGuilds(this IRestClient client,
        int limit, FetchDirection direction = FetchDirection.After, Snowflake? startFromId = null,
        IRestRequestOptions? options = null)
    {
        Guard.IsGreaterThanOrEqualTo(limit, 0);

        return PagedEnumerable.Create((state, cancellationToken) =>
        {
            var (client, limit, direction, startFromId, options) = state;
            return new FetchGuildsPagedEnumerator(client, limit, direction, startFromId, options, cancellationToken);
        }, (client, limit, direction, startFromId, options));
    }

    public static Task<IReadOnlyList<IPartialGuild>> FetchGuildsAsync(this IRestClient client,
        int limit = Discord.Limits.Rest.FetchGuildsPageSize, FetchDirection direction = FetchDirection.After, Snowflake? startFromId = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        Guard.IsGreaterThanOrEqualTo(limit, 0);

        if (limit == 0)
            return Task.FromResult(ReadOnlyList<IPartialGuild>.Empty);

        if (limit <= Discord.Limits.Rest.FetchGuildsPageSize)
            return client.InternalFetchGuildsAsync(limit, direction, startFromId, options, cancellationToken);

        var enumerable = client.EnumerateGuilds(limit, direction, startFromId, options);
        return enumerable.FlattenAsync(cancellationToken);
    }

    internal static async Task<IReadOnlyList<IPartialGuild>> InternalFetchGuildsAsync(this IRestClient client,
        int limit, FetchDirection direction, Snowflake? startFromId,
        IRestRequestOptions? options, CancellationToken cancellationToken)
    {
        var models = await client.ApiClient.FetchGuildsAsync(limit, direction, startFromId, options, cancellationToken).ConfigureAwait(false);
        return models.ToReadOnlyList(client, (model, client) => new TransientPartialGuild(client, model));
    }

    public static Task LeaveGuildAsync(this IRestClient client,
        Snowflake guildId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        return client.ApiClient.LeaveGuildAsync(guildId, options, cancellationToken);
    }

    public static async Task<IDirectChannel> CreateDirectChannelAsync(this IRestClient client,
        Snowflake userId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var channels = client.DirectChannels;
        if (channels != null && channels.TryGetValue(userId, out var cachedChannel))
            return cachedChannel;

        var content = new CreateDirectChannelJsonRestRequestContent
        {
            RecipientId = userId
        };

        var model = await client.ApiClient.CreateDirectChannelAsync(content, options, cancellationToken).ConfigureAwait(false);
        var channel = new TransientDirectChannel(client, model);

        if (channels != null && !channels.IsReadOnly)
            channels.Add(userId, channel);

        return channel;
    }
}
