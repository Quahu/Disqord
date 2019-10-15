using System.Threading.Tasks;

namespace Disqord.Rest
{
    public partial class RestDiscordClient : IDiscordClient
    {
        public Task<RestApplication> GetCurrentApplicationAsync()
            => CurrentApplication.DownloadAsync();
    }
}
