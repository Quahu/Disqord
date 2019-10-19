using System.Threading.Tasks;

namespace Disqord.Rest
{
    public partial class RestDiscordClient : IRestDiscordClient
    {
        public Task<RestApplication> GetCurrentApplicationAsync()
            => CurrentApplication.DownloadAsync();
    }
}
