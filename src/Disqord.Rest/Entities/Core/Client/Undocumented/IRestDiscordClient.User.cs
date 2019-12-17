using System;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    public partial interface IRestDiscordClient : IDisposable
    {
        Task<RestProfile> GetProfileAsync(Snowflake userId, RestRequestOptions options = null);

        Task SetNoteAsync(Snowflake userId, string note, RestRequestOptions options = null);

        Task<RestUserSettings> GetUserSettingsAsync(RestRequestOptions options = null);

        Task<RestUserSettings> ModifyUserSettingsAsync(Action<ModifyUserSettingsProperties> action, RestRequestOptions options = null);
    }
}
