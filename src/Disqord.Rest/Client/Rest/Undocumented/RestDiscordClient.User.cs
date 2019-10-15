using System.Threading.Tasks;

namespace Disqord.Rest
{
    public partial class RestDiscordClient : IDiscordClient
    {
        public async Task<RestUserProfile> GetUserProfileAsync(Snowflake userId, RestRequestOptions options = null)
        {
            var model = await ApiClient.GetUserProfileAsync(userId, options).ConfigureAwait(false);
            return new RestUserProfile(this, model);
        }

        public Task SetNoteAsync(Snowflake userId, string note, RestRequestOptions options = null)
            => ApiClient.CreateNoteAsync(userId, note, options);
    }
}
