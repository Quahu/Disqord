using System;
using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord
{
    public sealed partial class CachedVoiceChannel : CachedNestedChannel, IVoiceChannel
    {
        public async Task ModifyAsync(Action<ModifyVoiceChannelProperties> action, RestRequestOptions options = null)
        {
            var model = await Client.RestClient.InternalModifyChannelAsync(Id, action, options).ConfigureAwait(false);
            Update(model);
        }
    }
}
