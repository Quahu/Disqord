using System;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    public partial interface IRestDiscordClient : IDisposable
    {
        Task<RestCurrentUser> GetCurrentUserAsync(RestRequestOptions options = null);

        Task<RestUser> GetUserAsync(Snowflake userId, RestRequestOptions options = null);

        Task<RestCurrentUser> ModifyCurrentUserAsync(Action<ModifyCurrentUserProperties> action, RestRequestOptions options = null);

        // TODO: get current user guilds

        Task LeaveGuildAsync(Snowflake guildId, RestRequestOptions options = null);

        // TODO: get user dms

        Task<RestDmChannel> CreateDmChannelAsync(Snowflake userId, RestRequestOptions options = null);

        // TODO: create group dm
        // TODO: get user connections
    }
}
