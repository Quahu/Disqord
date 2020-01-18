using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Events;
using Disqord.Models;
using Disqord.Models.Dispatches;
using Disqord.Rest;
using Qommon.Collections;

namespace Disqord
{
    internal sealed partial class DiscordClientState
    {
        public Task HandleMessageReactionRemoveAllAsync(PayloadModel payload)
        {
            var model = Serializer.ToObject<MessageReactionRemoveAllModel>(payload.D);
            var channel = GetGuild(model.GuildId).GetTextChannel(model.ChannelId);
            var message = channel.GetMessage(model.MessageId);
            var reactions = message?._reactions.ToDictionary(x => x.Key, x => x.Value);
            message?._reactions.Clear();

            return _client._reactionsCleared.InvokeAsync(new ReactionsClearedEventArgs(
                channel,
                new DownloadableOptionalSnowflakeEntity<CachedMessage, RestMessage>(message, model.MessageId,
                    options => _client.GetMessageAsync(channel.Id, model.MessageId, options)),
                new ReadOnlyDictionary<IEmoji, ReactionData>(reactions ?? new Dictionary<IEmoji, ReactionData>())));
        }
    }
}
