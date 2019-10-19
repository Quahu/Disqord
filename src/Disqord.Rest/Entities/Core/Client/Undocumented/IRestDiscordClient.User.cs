using System;
using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord
{
    public partial interface IRestDiscordClient : IDisposable
    {
        Task<RestUserProfile> GetUserProfileAsync(Snowflake userId, RestRequestOptions options = null);

        Task SetNoteAsync(Snowflake userId, string note, RestRequestOptions options = null);

        Task<RestUserSettings> GetUserSettingsAsync(RestRequestOptions options = null);

        Task<RestUserSettings> ModifyUserSettingsAsync(Action<ModifyUserSettingsProperties> action, RestRequestOptions options = null);
    }
}
