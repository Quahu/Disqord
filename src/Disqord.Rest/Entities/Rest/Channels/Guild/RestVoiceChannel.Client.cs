using System;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    public sealed partial class RestVoiceChannel : RestNestedChannel, IVoiceChannel
    {
        public async Task ModifyAsync(Action<ModifyVoiceChannelProperties> action, RestRequestOptions options = null)
        {
            var model = await Client.InternalModifyChannelAsync(Id, action, options).ConfigureAwait(false);
            Update(model);
        }
    }
}
