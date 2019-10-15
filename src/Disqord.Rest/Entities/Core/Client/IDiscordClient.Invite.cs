using System;
using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord
{
    public partial interface IDiscordClient : IDisposable
    {
        Task<RestInvite> GetInviteAsync(string code, bool withCounts = true, RestRequestOptions options = null);

        Task<RestInvite> DeleteInviteAsync(string code, RestRequestOptions options = null);
    }
}
