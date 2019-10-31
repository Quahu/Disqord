using System.Threading.Tasks;
using Disqord.Events;
using Disqord.Models;

namespace Disqord
{
    internal sealed partial class DiscordClientState
    {
        public Task HandleChannelPinsUpdateAsync(PayloadModel payload)
        {
            // TODO
            //var model = Serializer.ToObject<ChannelModel>(payload.D);
            return _client._channelPinsUpdated.InvokeAsync(new ChannelPinsUpdatedEventArgs(_client));
        }
    }
}
