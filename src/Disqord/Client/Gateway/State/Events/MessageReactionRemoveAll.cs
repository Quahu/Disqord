using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Events;
using Disqord.Logging;
using Disqord.Models;
using Disqord.Models.Dispatches;
using Disqord.Rest;

namespace Disqord
{
    internal sealed partial class DiscordClientState
    {
        public Task HandleMessageReactionRemoveAllAsync(PayloadModel payload)
        {
            var model = Serializer.ToObject<MessageReactionRemoveAllModel>(payload.D);
            var channel = model.GuildId != null
                ? GetGuildChannel(model.ChannelId) as ICachedMessageChannel
                : GetPrivateChannel(model.ChannelId);

            if (channel == null)
            {
                Log(LogMessageSeverity.Warning, $"Uncached channel in MessageReactionRemoveAll. Id: {model.ChannelId}");
                return Task.CompletedTask;
            }

            var message = channel.GetMessage(model.MessageId);
            var reactions = message?._reactions.ToDictionary(x => x.Key, x => x.Value);
            message?._reactions.Clear();

            return _client._reactionsCleared.InvokeAsync(new ReactionsClearedEventArgs(
                channel,
                new DownloadableOptionalSnowflakeEntity<CachedMessage, RestMessage>(message, model.MessageId,
                    options => _client.GetMessageAsync(channel.Id, model.MessageId, options)),
                reactions ?? Optional<IReadOnlyDictionary<IEmoji, ReactionData>>.Empty));
        }
    }
}
