using System;
using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord
{
    public partial interface IDiscordClient : IDisposable
    {
        Task<RestUserProfile> GetUserProfileAsync(Snowflake userId, RestRequestOptions options = null);

        Task SetNoteAsync(Snowflake userId, string note, RestRequestOptions options = null);
    }
}
