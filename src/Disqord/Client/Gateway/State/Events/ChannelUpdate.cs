using System.Threading.Tasks;
using Disqord.Events;
using Disqord.Logging;
using Disqord.Models;

namespace Disqord
{
    internal sealed partial class DiscordClientState
    {
        public Task HandleChannelUpdateAsync(PayloadModel payload)
        {
            var model = Serializer.ToObject<ChannelModel>(payload.D);
            var channel = GetChannel(model.Id);
            if (channel == null)
            {
                _client.Log(LogMessageSeverity.Warning, $"Unknown channel in ChannelUpdate. Id: {model.Id}.");
                return Task.CompletedTask;
            }

            var before = channel.Clone();
            channel.Update(model);
            return _client._channelUpdated.InvokeAsync(new ChannelUpdatedEventArgs(before, channel));
        }
    }
}
