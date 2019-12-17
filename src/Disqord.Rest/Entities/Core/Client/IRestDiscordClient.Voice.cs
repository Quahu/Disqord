using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    public partial interface IRestDiscordClient : IDisposable
    {
        Task<IReadOnlyList<RestVoiceRegion>> GetVoiceRegionsAsync(RestRequestOptions options = null);
    }
}
