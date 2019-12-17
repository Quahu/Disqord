using System;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    public partial interface IRestDiscordClient : IDisposable
    {
        Task<RestInvite> GetInviteAsync(string code, bool withCounts = true, RestRequestOptions options = null);

        Task<RestInvite> DeleteInviteAsync(string code, RestRequestOptions options = null);
    }
}
