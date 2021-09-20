using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Qommon.Collections;
using Disqord.Rest.Api;
using Disqord.Rest.Pagination;

namespace Disqord.Rest
{
    public static partial class RestClientExtensions
    {
        public static async Task<ICurrentUser> FetchCurrentUserAsync(this IRestClient client, IRestRequestOptions options = null)
        {
            var model = await client.ApiClient.FetchCurrentUserAsync(options).ConfigureAwait(false);
            return new TransientCurrentUser(client, model);
        }

        public static async Task<IRestUser> FetchUserAsync(this IRestClient client, Snowflake userId, IRestRequestOptions options = null)
        {
            try
            {
                var model = await client.ApiClient.FetchUserAsync(userId, options).ConfigureAwait(false);
                return new TransientRestUser(client, model);
            }
            catch (RestApiException ex) when (ex.ErrorModel.Code == RestApiErrorCode.UnknownUser)
            {
                return null;
            }
        }

        public static async Task<ICurrentUser> ModifyCurrentUserAsync(this IRestClient client, Action<ModifyCurrentUserActionProperties> action, IRestRequestOptions options = null)
        {
            var properties = new ModifyCurrentUserActionProperties();
            action(properties);
            var content = new ModifyCurrentUserJsonRestRequestContent
            {
                Username = properties.Name,
                Avatar = properties.Avatar
            };

            var model = await client.ApiClient.ModifyCurrentUserAsync(content, options).ConfigureAwait(false);
            return new TransientCurrentUser(client, model);
        }

        public static IPagedEnumerable<IPartialGuild> EnumerateGuilds(this IRestClient client, int limit, RetrievalDirection direction = RetrievalDirection.Before, Snowflake? startFromId = null, IRestRequestOptions options = null)
        {
            var enumerator = new FetchGuildsPagedEnumerator(client, limit, direction, startFromId, options);
            return new PagedEnumerable<IPartialGuild>(enumerator);
        }

        public static Task<IReadOnlyList<IPartialGuild>> FetchGuildsAsync(this IRestClient client, int limit = 100, RetrievalDirection direction = RetrievalDirection.Before, Snowflake? startFromId = null, IRestRequestOptions options = null)
        {
            if (limit == 0)
                return Task.FromResult(ReadOnlyList<IPartialGuild>.Empty);

            if (limit <= 100)
                return client.InternalFetchGuildsAsync(limit, direction, startFromId, options);

            var enumerable = client.EnumerateGuilds(limit, direction, startFromId, options);
            return enumerable.FlattenAsync();
        }

        internal static async Task<IReadOnlyList<IPartialGuild>> InternalFetchGuildsAsync(this IRestClient client, int limit = 100, RetrievalDirection direction = RetrievalDirection.Before, Snowflake? startFromId = null, IRestRequestOptions options = null)
        {
            var models = await client.ApiClient.FetchGuildsAsync(limit, direction, startFromId, options).ConfigureAwait(false);
            return models.ToReadOnlyList(client, (x, client) => new TransientPartialGuild(client, x));
        }

        public static Task LeaveGuildAsync(this IRestClient client, Snowflake guildId, IRestRequestOptions options = null)
            => client.ApiClient.LeaveGuildAsync(guildId, options);

        public static async Task<IDirectChannel> CreateDirectChannelAsync(this IRestClient client, Snowflake userId, IRestRequestOptions options = null)
        {
            var channels = client.DirectChannels;
            if (channels != null && channels.TryGetValue(userId, out var cachedChannel))
                return cachedChannel;

            var content = new CreateDirectChannelJsonRestRequestContent
            {
                RecipientId = userId
            };

            var model = await client.ApiClient.CreateDirectChannelAsync(content, options).ConfigureAwait(false);
            var channel = new TransientDirectChannel(client, model);

            if (channels != null && !channels.IsReadOnly)
                channels.Add(userId, channel);

            return channel;
        }

        //public static async Task<IReadOnlyList<IConnection>> FetchConnectionsAsync(this IRestClient client, IRestRequestOptions options = null)
        //{
        //    var models = await client.ApiClient.FetchConnectionsAsync(options).ConfigureAwait(false);
        //    return models.ToReadOnlyList(client, (x, client) => new TransientConnection(client, x));
        //}
    }
}
