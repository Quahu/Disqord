using System;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    public partial interface IRestDiscordClient : IDisposable
    {
        Task<RestApplication> GetCurrentApplicationAsync(RestRequestOptions options = null);
    }
}
