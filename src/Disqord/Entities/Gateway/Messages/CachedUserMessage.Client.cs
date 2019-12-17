using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public sealed partial class CachedUserMessage : CachedMessage, IUserMessage
    {
        public async Task ModifyAsync(Action<ModifyMessageProperties> action, RestRequestOptions options = null)
        {
            var model = await Client.RestClient.InternalModifyMessageAsync(Channel.Id, Id, action, options).ConfigureAwait(false);
            Update(model);
        }

        public Task PinAsync(RestRequestOptions options = null)
            => Client.PinMessageAsync(Channel.Id, Id, options);

        public Task UnpinAsync(RestRequestOptions options = null)
            => Client.UnpinMessageAsync(Channel.Id, Id, options);
    }
}
