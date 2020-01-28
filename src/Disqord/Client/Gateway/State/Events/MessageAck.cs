using System.Threading.Tasks;
using Disqord.Events;
using Disqord.Models;
using Disqord.Models.Dispatches;

namespace Disqord
{
    internal sealed partial class DiscordClientState
    {
        public Task HandleMessageAckAsync(PayloadModel payload)
        {
            var model = Serializer.ToObject<MessageAckModel>(payload.D);
            if (!(GetChannel(model.ChannelId) is ICachedMessageChannel channel))
                return Task.CompletedTask;

            var message = channel.GetMessage(model.MessageId);
            return _client._messageAcknowledged.InvokeAsync(new MessageAcknowledgedEventArgs(channel,
                new SnowflakeOptional<CachedMessage>(message, model.MessageId)));
        }
    }
}
