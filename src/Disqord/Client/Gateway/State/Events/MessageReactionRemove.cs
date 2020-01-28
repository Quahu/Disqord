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
        public Task HandleMessageReactionRemoveAsync(PayloadModel payload)
        {
            var model = Serializer.ToObject<MessageReactionRemoveModel>(payload.D);
            var channel = model.GuildId != null
                ? GetGuildChannel(model.ChannelId) as ICachedMessageChannel
                : GetPrivateChannel(model.ChannelId);

            if (channel == null)
            {
                Log(LogMessageSeverity.Warning, $"Uncached channel in MessageReactionRemove. Id: {model.ChannelId}");
                return Task.CompletedTask;
            }

            var message = channel.GetMessage(model.MessageId);
            ReactionData reaction = null;
            if (message != null)
            {
                message._reactions.TryGetValue(model.Emoji.ToEmoji(), out reaction);
                if (reaction != null)
                {
                    var count = reaction.Count - 1;
                    if (count == 0)
                    {
                        message._reactions.TryRemove(reaction.Emoji, out _);
                    }
                    else
                    {
                        reaction.Count--;
                        if (model.UserId == _currentUser.Id)
                            reaction.HasCurrentUserReacted = false;
                    }
                }
            }

            var messageOptional = FetchableSnowflakeOptional.Create<CachedMessage, RestMessage, IMessage>(
                model.MessageId, message, RestFetchable.Create((this, model), (tuple, options) =>
                {
                    var (@this, model) = tuple;
                    return @this._client.GetMessageAsync(model.ChannelId, model.MessageId, options);
                }));
            var userOptional = FetchableSnowflakeOptional.Create<CachedUser, RestUser, IUser>(
                model.UserId, channel is CachedTextChannel textChannel
                    ? textChannel.Guild.GetMember(model.UserId) ?? GetUser(model.UserId)
                    : GetUser(model.UserId),
                RestFetchable.Create((this, model), async (tuple, options) =>
                {
                    var (@this, model) = tuple;
                    return model.GuildId != null
                        ? await @this._client.GetMemberAsync(model.GuildId.Value, model.UserId, options).ConfigureAwait(false)
                            ?? await @this._client.GetUserAsync(model.UserId, options).ConfigureAwait(false)
                        : await @this._client.GetUserAsync(model.UserId, options).ConfigureAwait(false);
                }));
            return _client._reactionRemoved.InvokeAsync(new ReactionRemovedEventArgs(
                channel, messageOptional, userOptional, reaction ?? Optional<ReactionData>.Empty, model.Emoji.ToEmoji()));
        }
    }
}
