using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Rest.Pagination;

namespace Disqord.Rest
{
    public static partial class RestEntityExtensions
    {
        public static Task<IGuildEvent> ModifyAsync(this IGuildEvent @event,
            Action<ModifyGuildEventActionProperties> action,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = @event.GetRestClient();
            return client.ModifyGuildEventAsync(@event.GuildId, @event.Id, action, options, cancellationToken);
        }

        public static Task DeleteAsync(this IGuildEvent @event,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = @event.GetRestClient();
            return client.DeleteGuildEventAsync(@event.GuildId, @event.Id, options, cancellationToken);
        }

        public static IPagedEnumerable<IUser> EnumerateUsers(this IGuildEvent @event,
            int limit, RetrievalDirection direction = RetrievalDirection.After, Snowflake? startFromId = null,
            bool? withMember = null,
            IRestRequestOptions options = null)
        {
            var client = @event.GetRestClient();
            return client.EnumerateGuildEventUsers(@event.GuildId, @event.Id, limit, direction, startFromId, withMember, options);
        }

        public static Task<IReadOnlyList<IUser>> FetchUsersAsync(this IGuildEvent @event,
            int limit = Discord.Limits.Rest.FetchGuildEventUsersPageSize,
            RetrievalDirection direction = RetrievalDirection.After, Snowflake? startFromId = null,
            bool? withMember = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = @event.GetRestClient();
            return client.FetchGuildEventUsersAsync(@event.GuildId, @event.Id, limit, direction, startFromId, withMember, options, cancellationToken);
        }
    }
}
