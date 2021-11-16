using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord.Rest.Api
{
    public static partial class RestApiClientExtensions
    {
        public static Task<GuildScheduledEventJsonModel[]> FetchGuildEventsAsync(this IRestApiClient client, Snowflake guildId, bool? withUserCount = null, IRestRequestOptions options = null)
        {
            var queryParameters = withUserCount != null
                ? new[] { new KeyValuePair<string, object>("with_user_count", withUserCount.Value) }
                : null;

            var route = Format(Route.GuildEvents.GetEvents, queryParameters, guildId);
            return client.ExecuteAsync<GuildScheduledEventJsonModel[]>(route, null, options);
        }

        public static Task<GuildScheduledEventJsonModel> FetchGuildEventAsync(this IRestApiClient client, Snowflake eventId, IRestRequestOptions options = null)
        {
            var route = Format(Route.GuildEvents.GetEvent, eventId);
            return client.ExecuteAsync<GuildScheduledEventJsonModel>(route, null, options);
        }

        public static Task<GuildScheduledEventJsonModel> CreateGuildEventAsync(this IRestApiClient client, Snowflake guildId, CreateGuildEventJsonRestRequestContent content, IRestRequestOptions options = null)
        {
            var route = Format(Route.GuildEvents.CreateEvent, guildId);
            return client.ExecuteAsync<GuildScheduledEventJsonModel>(route, content, options);
        }

        public static Task<GuildScheduledEventJsonModel> ModifyGuildEventAsync(this IRestApiClient client, Snowflake eventId, ModifyGuildEventJsonRestRequestContent content, IRestRequestOptions options = null)
        {
            var route = Format(Route.GuildEvents.ModifyEvent, eventId);
            return client.ExecuteAsync<GuildScheduledEventJsonModel>(route, content, options);
        }

        public static Task DeleteGuildEventAsync(this IRestApiClient client, Snowflake eventId, IRestRequestOptions options = null)
        {
            var route = Format(Route.GuildEvents.DeleteEvent, eventId);
            return client.ExecuteAsync(route, null, options);
        }
    }

}