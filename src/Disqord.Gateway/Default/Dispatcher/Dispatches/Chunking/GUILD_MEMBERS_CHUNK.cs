using System;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class GuildMembersChunkHandler : Handler<GuildMembersChunkJsonModel, EventArgs>
    {
        public override async ValueTask<EventArgs> HandleDispatchAsync(IGatewayApiClient shard, GuildMembersChunkJsonModel model)
        {
            await Dispatcher.Client.Chunker.HandleChunkAsync(model).ConfigureAwait(false);
            return null;
        }
    }
}
