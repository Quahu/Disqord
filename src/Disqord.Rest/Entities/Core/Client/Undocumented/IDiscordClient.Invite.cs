using System;
using System.Threading.Tasks;

namespace Disqord
{
    public partial interface IDiscordClient : IDisposable
    {
        Task AcceptInviteAsync(string code, RestRequestOptions options = null);
    }
}
