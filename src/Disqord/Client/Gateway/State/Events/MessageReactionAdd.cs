using System;
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
        public Task HandleMessageReactionAddAsync(PayloadModel payload)
        {
            var model = Serializer.ToObject<MessageReactionAddModel>(payload.D);
            var channel = model.GuildId != null
                ? GetGuildChannel(model.ChannelId) as ICachedMessageChannel
                : GetPrivateChannel(model.ChannelId);

            if (channel == null)
            {
                Log(LogMessageSeverity.Warning, $"Uncached channel in MessageReactionAdd. Id: {model.ChannelId}");
                return Task.CompletedTask;
            }

            var message = channel.GetMessage(model.MessageId);
            IEmoji emoji;
            ReactionData reaction = null;
            if (message != null)
            {
                if (message._reactions.TryGetValue(model.Emoji.ToEmoji(), out reaction))
                {
                    reaction.Count++;
                    if (model.UserId == _currentUser.Id)
                        reaction.HasCurrentUserReacted = true;
                }
                else
                {
                    reaction = new ReactionData(new ReactionModel
                    {
                        Count = 1,
                        Me = model.UserId == _currentUser.Id,
                        Emoji = model.Emoji
                    });
                    message._reactions.TryAdd(reaction.Emoji, reaction);
                }
                emoji = reaction.Emoji;
            }
            else
            {
                emoji = model.Emoji.ToEmoji();
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
            return _client._reactionAdded.InvokeAsync(new ReactionAddedEventArgs(
                channel, messageOptional, userOptional, reaction ?? Optional<ReactionData>.Empty, emoji));
        }
    }
}
