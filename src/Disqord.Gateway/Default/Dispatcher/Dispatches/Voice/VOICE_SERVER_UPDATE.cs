using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Default.Dispatcher;

public class VoiceServerUpdateDispatchHandler : DispatchHandler<VoiceServerUpdateJsonModel, VoiceServerUpdatedEventArgs>
{
    public override ValueTask<VoiceServerUpdatedEventArgs?> HandleDispatchAsync(IShard shard, VoiceServerUpdateJsonModel model)
    {
        var e = new VoiceServerUpdatedEventArgs(model.GuildId, model.Token, model.Endpoint);
        return new(e);
    }
}
