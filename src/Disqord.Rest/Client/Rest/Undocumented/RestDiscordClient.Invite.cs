using System.Threading.Tasks;

namespace Disqord.Rest
{
    public partial class RestDiscordClient : IDiscordClient
    {
        public Task AcceptInviteAsync(string code, RestRequestOptions options = null)
            => ApiClient.AcceptInviteAsync(code, options);
    }
}
