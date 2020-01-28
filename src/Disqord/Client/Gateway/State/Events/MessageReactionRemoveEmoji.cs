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

            var messageOptional = FetchableSnowflakeOptional.Create<CachedMessage, RestMessage, IMessage>(
                model.MessageId, message, RestFetchable.Create((this, model), (tuple, options) =>
                {
                    var (@this, model) = tuple;
                    return @this._client.GetMessageAsync(model.ChannelId, model.MessageId, options);
                }));
            return _client._emojiReactionsCleared.InvokeAsync(new EmojiReactionsClearedEventArgs(
                channel, messageOptional, emoji, data));
        }
    }
}
