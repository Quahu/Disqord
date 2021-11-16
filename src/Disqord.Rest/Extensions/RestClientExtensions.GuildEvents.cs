using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Qommon.Collections;
using Disqord.Rest.Api;

namespace Disqord.Rest
{
    public static partial class RestClientExtensions
    {
        public static async Task<IReadOnlyList<IGuildEvent>> FetchGuildEventsAsync(this IRestClient client, Snowflake guildId, bool? withUserCount = null, IRestRequestOptions options = null)
        {
            var models = await client.ApiClient.FetchGuildEventsAsync(guildId, withUserCount, options).ConfigureAwait(false);
            return models.ToReadOnlyList(client, static (x, client) => new TransientGuildEvent(client, x));
        }

        public static async Task<IGuildEvent> FetchGuildEventAsync(this IRestClient client, Snowflake eventId, IRestRequestOptions options = null)
        {
            var model = await client.ApiClient.FetchGuildEventAsync(eventId, options).ConfigureAwait(false);
            return new TransientGuildEvent(client, model);
        }

        public static async Task<IGuildEvent> CreateGuildEventAsync(this IRestClient client, Snowflake guildId, string name, PrivacyLevel privacyLevel, DateTimeOffset startTime, GuildEventTargetType entityType, Action<CreateGuildEventActionProperties> action = null, IRestRequestOptions options = null)
        {
            var properties = new CreateGuildEventActionProperties();
            action?.Invoke(properties);

            var content = new CreateGuildEventJsonRestRequestContent
            {
                ChannelId = properties.ChannelId,
                Name = name,
                PrivacyLevel = privacyLevel,
                ScheduledStartTime = startTime,
                Description = properties.Description,
                EntityType = entityType
            };

            var model = await client.ApiClient.CreateGuildEventAsync(guildId, content, options).ConfigureAwait(false);
            return new TransientGuildEvent(client, model);
        }

        public static async Task<IGuildEvent> ModifyGuildEventAsync(this IRestClient client, Snowflake eventId, Action<ModifyGuildEventActionProperties> action, IRestRequestOptions options = null)
        {
            var properties = new ModifyGuildEventActionProperties();
            action?.Invoke(properties);

            var content = new ModifyGuildEventJsonRestRequestContent
            {
                ChannelId = properties.ChannelId,
                Name = properties.Name,
                PrivacyLevel = properties.PrivacyLevel,
                ScheduledStartTime = properties.ScheduledStartTime,
                Description = properties.Description,
                EntityType = properties.EntityType
            };

            var model = await client.ApiClient.ModifyGuildEventAsync(eventId, content, options).ConfigureAwait(false);
            return new TransientGuildEvent(client, model);
        }

        public static Task DeleteGuildEventAsync(this IRestClient client, Snowflake eventId, IRestRequestOptions options = null)
            => client.ApiClient.DeleteGuildEventAsync(eventId, options);

    }
}
