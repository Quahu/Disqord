using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Models;
using Qommon;

namespace Disqord.Rest.Api
{
    public static partial class RestApiClientExtensions
    {
        public static Task<GuildScheduledEventJsonModel[]> FetchGuildScheduledEventsAsync(this IRestApiClient client,
            Snowflake guildId, bool? withSubscriberCount = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            FormattedRoute route;
            if (withSubscriberCount != null)
            {
                var queryParameters = new[] { new KeyValuePair<string, object>("with_user_count", withSubscriberCount.Value) };

                route = Format(Route.GuildEvents.GetEvents, queryParameters, guildId);
            }
            else
            {
                route = Format(Route.GuildEvents.GetEvents, guildId);
            }

            return client.ExecuteAsync<GuildScheduledEventJsonModel[]>(route, null, options, cancellationToken);
        }

        public static Task<GuildScheduledEventJsonModel> CreateGuildScheduledEventAsync(this IRestApiClient client,
            Snowflake guildId, CreateGuildScheduledEventJsonRestRequestContent content,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var route = Format(Route.GuildEvents.CreateEvent, guildId);
            return client.ExecuteAsync<GuildScheduledEventJsonModel>(route, content, options, cancellationToken);
        }

        public static Task<GuildScheduledEventJsonModel> FetchGuildScheduledEventAsync(this IRestApiClient client,
            Snowflake guildId, Snowflake eventId, bool? withUserCount = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            FormattedRoute route;
            if (withUserCount != null)
            {
                var queryParameters = new[] { new KeyValuePair<string, object>("with_user_count", withUserCount.Value) };

                route = Format(Route.GuildEvents.GetEvent, queryParameters, guildId, eventId);
            }
            else
            {
                route = Format(Route.GuildEvents.GetEvent, guildId, eventId);
            }

            return client.ExecuteAsync<GuildScheduledEventJsonModel>(route, null, options, cancellationToken);
        }

        public static Task<GuildScheduledEventJsonModel> ModifyGuildScheduledEventAsync(this IRestApiClient client,
            Snowflake guildId, Snowflake eventId, ModifyGuildScheduledEventJsonRestRequestContent content,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var route = Format(Route.GuildEvents.ModifyEvent, guildId, eventId);
            return client.ExecuteAsync<GuildScheduledEventJsonModel>(route, content, options, cancellationToken);
        }

        public static Task DeleteGuildScheduledEventAsync(this IRestApiClient client,
            Snowflake guildId, Snowflake eventId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var route = Format(Route.GuildEvents.DeleteEvent, guildId, eventId);
            return client.ExecuteAsync(route, null, options, cancellationToken);
        }

        public static Task<GuildScheduledEventUserJsonModel[]> FetchGuildScheduledEventUsersAsync(this IRestApiClient client,
            Snowflake guildId, Snowflake eventId, int limit = Discord.Limits.Rest.FetchGuildEventUsersPageSize,
            RetrievalDirection direction = RetrievalDirection.After, Snowflake? startFromId = null,
            bool? withMember = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            Guard.IsBetweenOrEqualTo(limit, 0, Discord.Limits.Rest.FetchGuildEventUsersPageSize);

            var queryParameters = new Dictionary<string, object>
            {
                ["limit"] = limit
            };

            if (startFromId != null)
            {
                switch (direction)
                {
                    case RetrievalDirection.Before:
                    {
                        queryParameters["before"] = startFromId;
                        break;
                    }
                    case RetrievalDirection.After:
                    {
                        queryParameters["after"] = startFromId;
                        break;
                    }
                    default:
                    {
                        throw new ArgumentOutOfRangeException(nameof(direction), "Invalid guild event user fetch direction.");
                    }
                }
            }

            if (withMember != null)
                queryParameters["with_member"] = withMember.Value;

            var route = Format(Route.GuildEvents.GetEventUsers, queryParameters, guildId, eventId);
            return client.ExecuteAsync<GuildScheduledEventUserJsonModel[]>(route, null, options, cancellationToken);
        }
    }
}
