using System.Threading.Tasks;
using Disqord.Events;
using Disqord.Logging;
using Disqord.Models;
using Disqord.Models.Dispatches;

namespace Disqord
{
    internal sealed partial class DiscordClientState
    {
        public Task HandleMessageDeleteAsync(PayloadModel payload)
        {
            var model = Serializer.ToObject<MessageDeleteModel>(payload.D);
            var channel = model.GuildId != null
                ? GetGuildChannel(model.ChannelId) as ICachedMessageChannel
                : GetPrivateChannel(model.ChannelId);

            if (channel == null)
            {
                Log(LogMessageSeverity.Warning, $"Uncached channel in MessageDeleted. Id: {model.ChannelId}");
                return Task.CompletedTask;
            }

            CachedUserMessage message = null;
            _messageCache?.TryRemoveMessage(channel.Id, model.Id, out message);

            return _client._messageDeleted.InvokeAsync(new MessageDeletedEventArgs(channel,
                new SnowflakeOptional<CachedUserMessage>(message, model.Id)));
        }
    }
}
