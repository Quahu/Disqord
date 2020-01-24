using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    public partial interface IRestDiscordClient : IDisposable
    {
        Task<RestCurrentUser> GetCurrentUserAsync(RestRequestOptions options = null);

        Task<RestUser> GetUserAsync(Snowflake userId, RestRequestOptions options = null);

        Task<RestCurrentUser> ModifyCurrentUserAsync(Action<ModifyCurrentUserProperties> action, RestRequestOptions options = null);

        RestRequestEnumerable<RestPartialGuild> GetGuildsEnumerable(int limit, RetrievalDirection direction = RetrievalDirection.Before, Snowflake? startFromId = null, RestRequestOptions options = null);

        Task<IReadOnlyList<RestPartialGuild>> GetGuildsAsync(int limit = 100, RetrievalDirection direction = RetrievalDirection.Before, Snowflake? startFromId = null, RestRequestOptions options = null);

        Task LeaveGuildAsync(Snowflake guildId, RestRequestOptions options = null);

        Task<IReadOnlyList<RestPrivateChannel>> GetPrivateChannelsAsync(RestRequestOptions options = null);

        Task<RestDmChannel> CreateDmChannelAsync(Snowflake userId, RestRequestOptions options = null);

        // TODO: create group dm

        Task<IReadOnlyList<RestConnection>> GetConnectionsAsync(RestRequestOptions options = null);
    }
}
