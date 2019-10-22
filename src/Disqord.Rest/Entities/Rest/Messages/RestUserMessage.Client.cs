using System;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    public sealed partial class RestUserMessage : RestMessage, IUserMessage
    {
        public async Task ModifyAsync(Action<ModifyMessageProperties> action, RestRequestOptions options = null)
        {
            var model = await Client.InternalModifyMessageAsync(ChannelId, Id, action, options).ConfigureAwait(false);
            Update(model);
        }

        public Task PinAsync(RestRequestOptions options = null)
            => Client.PinMessageAsync(ChannelId, Id, options);

        public Task UnpinAsync(RestRequestOptions options = null)
            => Client.UnpinMessageAsync(ChannelId, Id, options);
    }
}
