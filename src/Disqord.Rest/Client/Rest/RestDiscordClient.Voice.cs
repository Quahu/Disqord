using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    public partial class RestDiscordClient : IRestDiscordClient
    {
        public async Task<IReadOnlyList<RestVoiceRegion>> GetVoiceRegionsAsync(RestRequestOptions options = null)
        {
            var models = await ApiClient.ListVoiceRegionsAsync(options).ConfigureAwait(false);
            return models.Select(x => new RestVoiceRegion(this, x)).ToImmutableArray();
        }
    }
}
