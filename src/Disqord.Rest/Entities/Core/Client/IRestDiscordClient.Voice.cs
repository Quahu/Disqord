using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord
{
    public partial interface IRestDiscordClient : IDisposable
    {
        Task<IReadOnlyList<RestVoiceRegion>> GetVoiceRegionsAsync(RestRequestOptions options = null);
    }
}
