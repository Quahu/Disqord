using System;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Default.Dispatcher;

public class GuildMembersChunkDispatchHandler : DispatchHandler<GuildMembersChunkJsonModel, EventArgs>
{
    public override async ValueTask<EventArgs?> HandleDispatchAsync(IShard shard, GuildMembersChunkJsonModel model)
    {
        await Dispatcher.Client.Chunker.OnChunk(model).ConfigureAwait(false);
        return null;
    }
}
