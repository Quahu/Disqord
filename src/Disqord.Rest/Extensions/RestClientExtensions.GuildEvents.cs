using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Models;
using Qommon.Collections;
using Disqord.Rest.Api;
using Disqord.Rest.Pagination;
using Qommon;
using Qommon.Collections.ReadOnly;

namespace Disqord.Rest
{
    public static partial class RestClientExtensions
    {
        public static async Task<IReadOnlyList<IGuildEvent>> FetchGuildEventsAsync(this IRestClient client,
            Snowflake guildId, bool? withSubscriberCount = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var models = await client.ApiClient.FetchGuildScheduledEventsAsync(guildId, withSubscriberCount, options, cancellationToken).ConfigureAwait(false);
            return models.ToReadOnlyList(client, static (x, client) => new TransientGuildEvent(client, x));
        }

        public static async Task<IGuildEvent> CreateGuildEventAsync(this IRestClient client,
            Snowflake guildId, string name, DateTimeOffset startsAt, GuildEventTargetType targetType,
            PrivacyLevel privacyLevel = PrivacyLevel.GuildOnly, Action<CreateGuildEventActionProperties> action = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var properties = new CreateGuildEventActionProperties();
            action?.Invoke(properties);

            var content = new CreateGuildScheduledEventJsonRestRequestContent
            {
                ChannelId = properties.ChannelId,
                Name = name,
                PrivacyLevel = privacyLevel,
                ScheduledStartTime = startsAt,
                ScheduledEndTime = properties.EndsAt,
                Description = properties.Description,
                EntityType = targetType,
                Image = properties.CoverImage
            };

            if (properties.Location.HasValue)
            {
                content.EntityMetadata = new GuildScheduledEventEntityMetadataJsonModel
                {
                    Location = properties.Location.Value
                };
            }

            var model = await client.ApiClient.CreateGuildScheduledEventAsync(guildId, content, options, cancellationToken).ConfigureAwait(false);
            return new TransientGuildEvent(client, model);
        }

        public static async Task<IGuildEvent> FetchGuildEventAsync(this IRestClient client,
            Snowflake guildId, Snowflake eventId, bool? withUserCount = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var model = await client.ApiClient.FetchGuildScheduledEventAsync(guildId, eventId, withUserCount, options, cancellationToken).ConfigureAwait(false);
            return new TransientGuildEvent(client, model);
        }

        public static async Task<IGuildEvent> ModifyGuildEventAsync(this IRestClient client,
            Snowflake guildId, Snowflake eventId, Action<ModifyGuildEventActionProperties> action,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var content = action.ToContent();
            var model = await client.ApiClient.ModifyGuildScheduledEventAsync(guildId, eventId, content, options, cancellationToken).ConfigureAwait(false);
            return new TransientGuildEvent(client, model);
        }

        public static Task DeleteGuildEventAsync(this IRestClient client,
            Snowflake guildId, Snowflake eventId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
            => client.ApiClient.DeleteGuildScheduledEventAsync(guildId, eventId, options, cancellationToken);

        public static IPagedEnumerable<IUser> EnumerateGuildEventUsers(this IRestClient client,
            Snowflake guildId, Snowflake eventId, int limit, RetrievalDirection direction = RetrievalDirection.After,
            Snowflake? startFromId = null, bool? withMember = null,
            IRestRequestOptions options = null)
        {
            Guard.IsGreaterThanOrEqualTo(limit, 0);

            return PagedEnumerable.Create((state, cancellationToken) =>
            {
                var (client, guildId, eventId, limit, direction, startFromId, withMember, options) = state;
                return new FetchGuildEventUsersPagedEnumerator(client, guildId, eventId, limit, direction, startFromId, withMember, options, cancellationToken);
            }, (client, guildId, eventId, limit, direction, startFromId, withMember, options));
        }

        public static Task<IReadOnlyList<IUser>> FetchGuildEventUsersAsync(this IRestClient client,
            Snowflake guildId, Snowflake eventId, int limit = Discord.Limits.Rest.FetchGuildEventUsersPageSize,
            RetrievalDirection direction = RetrievalDirection.After, Snowflake? startFromId = null,
            bool? withMember = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            if (limit == 0)
                return Task.FromResult(ReadOnlyList<IUser>.Empty);

            if (limit <= Discord.Limits.Rest.FetchGuildEventUsersPageSize)
                return client.InternalFetchGuildEventUsersAsync(guildId, eventId, limit, direction, startFromId, withMember, options, cancellationToken);

            var enumerator = client.EnumerateGuildEventUsers(guildId, eventId, limit, direction, startFromId, withMember, options);
            return enumerator.FlattenAsync(cancellationToken);
        }

        internal static async Task<IReadOnlyList<IUser>> InternalFetchGuildEventUsersAsync(this IRestClient client,
            Snowflake guildId, Snowflake eventId, int limit, RetrievalDirection direction, Snowflake? startFromId = null,
            bool? withMember = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var models = await client.ApiClient.FetchGuildScheduledEventUsersAsync(guildId, eventId, limit, direction, startFromId, withMember, options, cancellationToken).ConfigureAwait(false);
            return models.ToReadOnlyList((client, guildId), static (x, tuple) =>
            {
                var (client, guildId) = tuple;

                if (x.Member.HasValue)
                {
                    x.Member.Value.User = x.User;
                    return new TransientMember(client, guildId, x.Member.Value);
                }

                return new TransientUser(client, x.User);
            });
        }
    }
}
