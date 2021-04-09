using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord.Rest.Api
{
    public static partial class RestApiClientExtensions
    {
        public static Task<UserJsonModel> FetchCurrentUserAsync(this IRestApiClient client, IRestRequestOptions options = null)
        {
            var route = Format(Route.User.GetCurrentUser);
            return client.ExecuteAsync<UserJsonModel>(route, null, options);
        }

        public static Task<UserJsonModel> FetchUserAsync(this IRestApiClient client, Snowflake userId, IRestRequestOptions options = null)
        {
            var route = Format(Route.User.GetUser, userId);
            return client.ExecuteAsync<UserJsonModel>(route, null, options);
        }

        public static Task<UserJsonModel> ModifyCurrentUserAsync(this IRestApiClient client, ModifyCurrentUserJsonRestRequestContent content, IRestRequestOptions options = null)
        {
            var route = Format(Route.User.ModifyCurrentUser);
            return client.ExecuteAsync<UserJsonModel>(route, content, options);
        }

        public static Task<GuildJsonModel[]> FetchGuildsAsync(this IRestApiClient client, int limit = 100, RetrievalDirection direction = RetrievalDirection.Before, Snowflake? startFromId = null, IRestRequestOptions options = null)
        {
            if (limit < 1 || limit > 100)
                throw new ArgumentOutOfRangeException(nameof(limit));

            var queryParameters = new Dictionary<string, object>(startFromId != null ? 2 : 1)
            {
                ["limit"] = limit
            };

            switch (direction)
            {
                case RetrievalDirection.Before:
                {
                    if (startFromId != null)
                        queryParameters["before"] = startFromId;

                    break;
                }
                case RetrievalDirection.After:
                {
                    queryParameters["after"] = startFromId ?? throw new ArgumentNullException(nameof(startFromId), "The ID to fetch guilds after must not be null.");
                    break;
                }
                default:
                {
                    throw new ArgumentOutOfRangeException(nameof(direction), "Invalid guild fetch direction.");
                }
            }

            var route = Format(Route.User.GetGuilds, queryParameters);
            return client.ExecuteAsync<GuildJsonModel[]>(route, null, options);
        }

        public static Task LeaveGuildAsync(this IRestApiClient client, Snowflake guildId, IRestRequestOptions options = null)
        {
            var route = Format(Route.User.LeaveGuild, guildId);
            return client.ExecuteAsync(route, null, options);
        }

        public static Task<ChannelJsonModel> CreateDirectChannelAsync(this IRestApiClient client, CreateDirectChannelJsonRestRequestContent content, IRestRequestOptions options = null)
        {
            var route = Format(Route.User.CreateDirectChannel);
            return client.ExecuteAsync<ChannelJsonModel>(route, content, options);
        }

        public static Task<ConnectionJsonModel[]> FetchConnectionsAsync(this IRestApiClient client, IRestRequestOptions options = null)
        {
            var route = Format(Route.User.CreateDirectChannel);
            return client.ExecuteAsync<ConnectionJsonModel[]>(route, null, options);
        }
    }
}
