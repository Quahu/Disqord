using System;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    public partial interface IRestDiscordClient : IDisposable
    {
        Task AcceptInviteAsync(string code, RestRequestOptions options = null);
    }
}
