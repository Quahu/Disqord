using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Models;
using Qommon;

namespace Disqord.Rest.Api;

public static partial class RestApiClientExtensions
{
    public static Task<UserJsonModel> FetchCurrentUserAsync(this IRestApiClient client,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.User.GetCurrentUser);
        return client.ExecuteAsync<UserJsonModel>(route, null, options, cancellationToken);
    }

    public static Task<UserJsonModel> FetchUserAsync(this IRestApiClient client,
        Snowflake userId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.User.GetUser, userId);
        return client.ExecuteAsync<UserJsonModel>(route, null, options, cancellationToken);
    }

    public static Task<UserJsonModel> ModifyCurrentUserAsync(this IRestApiClient client,
        ModifyCurrentUserJsonRestRequestContent content,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.User.ModifyCurrentUser);
        return client.ExecuteAsync<UserJsonModel>(route, content, options, cancellationToken);
    }

    public static Task<GuildJsonModel[]> FetchGuildsAsync(this IRestApiClient client,
        int limit = Discord.Limits.Rest.FetchGuildsPageSize, FetchDirection direction = FetchDirection.After, Snowflake? startFromId = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        Guard.IsBetweenOrEqualTo(limit, 0, Discord.Limits.Rest.FetchGuildsPageSize);

        var queryParameters = new Dictionary<string, object>(startFromId != null ? 2 : 1)
        {
            ["limit"] = limit
        };

        switch (direction)
        {
            case FetchDirection.Before:
            {
                if (startFromId != null)
                    queryParameters["before"] = startFromId;

                break;
            }
            case FetchDirection.After:
            {
                if (startFromId != null)
                    queryParameters["after"] = startFromId;

                break;
            }
            default:
            {
                throw new ArgumentOutOfRangeException(nameof(direction), "Invalid guild fetch direction.");
            }
        }

        var route = Format(Route.User.GetGuilds, queryParameters);
        return client.ExecuteAsync<GuildJsonModel[]>(route, null, options, cancellationToken);
    }

    public static Task<MemberJsonModel> FetchCurrentGuildMemberAsync(this IRestApiClient client,
        Snowflake guildId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.User.GetCurrentGuildMember, guildId);
        return client.ExecuteAsync<MemberJsonModel>(route, null, options, cancellationToken);
    }

    public static Task LeaveGuildAsync(this IRestApiClient client,
        Snowflake guildId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.User.LeaveGuild, guildId);
        return client.ExecuteAsync(route, null, options, cancellationToken);
    }

    public static Task<ChannelJsonModel> CreateDirectChannelAsync(this IRestApiClient client,
        CreateDirectChannelJsonRestRequestContent content,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.User.CreateDirectChannel);
        return client.ExecuteAsync<ChannelJsonModel>(route, content, options, cancellationToken);
    }

    public static Task<ConnectionJsonModel[]> FetchConnectionsAsync(this IRestApiClient client,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.User.GetConnections);
        return client.ExecuteAsync<ConnectionJsonModel[]>(route, null, options, cancellationToken);
    }
}
