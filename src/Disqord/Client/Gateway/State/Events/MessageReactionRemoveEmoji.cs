using System.Threading.Tasks;
using Disqord.Events;
using Disqord.Models;
using Disqord.Models.Dispatches;
using Disqord.Rest;

namespace Disqord
{
    internal sealed partial class DiscordClientState
    {
        public Task HandleMessageReactionRemoveEmojiAsync(PayloadModel payload)
        {
            // TODO: intents will ruin everything
            var model = Serializer.ToObject<MessageReactionRemoveEmojiModel>(payload.D);
            var channel = GetGuild(model.GuildId).GetTextChannel(model.ChannelId);
            var message = channel.GetMessage(model.MessageId);
            var emoji = model.Emoji.ToEmoji();
            ReactionData data = null;
            message?._reactions.TryRemove(emoji, out data);

            return _client._emojiReactionsCleared.InvokeAsync(new EmojiReactionsClearedEventArgs(
                channel,
                new DownloadableOptionalSnowflakeEntity<CachedMessage, RestMessage>(message, model.MessageId,
                    options => _client.GetMessageAsync(channel.Id, model.MessageId, options)),
                emoji,
                data));
        }
    }
}
