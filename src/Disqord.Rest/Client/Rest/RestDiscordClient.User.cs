using System;
using System.Net;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    public partial class RestDiscordClient : IDiscordClient
    {
        public Task<RestCurrentUser> GetCurrentUserAsync(RestRequestOptions options = null)
            => CurrentUser.DownloadAsync(options);

        public async Task<RestUser> GetUserAsync(Snowflake userId, RestRequestOptions options = null)
        {
            try
            {
                var model = await ApiClient.GetUserAsync(userId, options).ConfigureAwait(false);
                return new RestUser(this, model);
            }
            catch (HttpDiscordException ex) when (ex.HttpStatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<RestCurrentUser> ModifyCurrentUserAsync(Action<ModifyCurrentUserProperties> action, RestRequestOptions options = null)
        {
            var properties = new ModifyCurrentUserProperties();
            action(properties);
            var model = await ApiClient.ModifyCurrentUserAsync(properties, options).ConfigureAwait(false);
            var user = new RestCurrentUser(this, model);
            if (!CurrentUser.HasValue)
                CurrentUser.SetValue(user);
            else
                CurrentUser.Value.Update(model);

            return user;
        }

        public Task LeaveGuildAsync(Snowflake guildId, RestRequestOptions options = null)
            => ApiClient.LeaveGuildAsync(guildId, options);

        public async Task<RestDmChannel> CreateDmChannelAsync(Snowflake userId, RestRequestOptions options = null)
        {
            var model = await ApiClient.CreateDmAsync(userId, options).ConfigureAwait(false);
            return new RestDmChannel(this, model);
        }
    }
}
