using System;
using System.Threading.Tasks;

namespace Disqord
{
    public partial interface IRestDiscordClient : IDisposable
    {
        Task AcceptInviteAsync(string code, RestRequestOptions options = null);
    }
}
