using System;
using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord.Rest
{
    public partial class RestDiscordClient : IRestDiscordClient
    {
        public async Task<RestProfile> GetProfileAsync(Snowflake userId, RestRequestOptions options = null)
        {
            var model = await ApiClient.GetUserProfileAsync(userId, options).ConfigureAwait(false);
            return new RestProfile(this, model);
        }

        public Task SetNoteAsync(Snowflake userId, string note, RestRequestOptions options = null)
            => ApiClient.CreateNoteAsync(userId, note, options);

        public async Task<RestUserSettings> GetUserSettingsAsync(RestRequestOptions options = null)
        {
            var model = await ApiClient.GetUserSettingsAsync(options).ConfigureAwait(false);
            return new RestUserSettings(this, model);
        }

        public async Task<RestUserSettings> ModifyUserSettingsAsync(Action<ModifyUserSettingsProperties> action, RestRequestOptions options = null)
        {
            var model = await InternalModifyUserSettingsAsync(action, options).ConfigureAwait(false);
            return new RestUserSettings(this, model);
        }

        internal async Task<UserSettingsModel> InternalModifyUserSettingsAsync(Action<ModifyUserSettingsProperties> action, RestRequestOptions options = null)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var properties = new ModifyUserSettingsProperties();
            action(properties);
            var model = await ApiClient.ModifyUserSettingsAsync(properties, options).ConfigureAwait(false);
            return model;
        }
    }
}
