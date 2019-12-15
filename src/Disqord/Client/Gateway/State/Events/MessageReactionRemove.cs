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

            return _client._reactionRemoved.InvokeAsync(
                new ReactionRemovedEventArgs(
                    channel,
                    new DownloadableOptionalSnowflakeEntity<CachedMessage, RestMessage>(message, model.MessageId,
                        options => _client.GetMessageAsync(channel.Id, model.MessageId, options)),
                    new DownloadableOptionalSnowflakeEntity<CachedUser, RestUser>(
                        channel is CachedTextChannel textChannel
                            ? textChannel.Guild.GetMember(model.UserId)
                                ?? GetUser(model.UserId)
                            : GetUser(model.UserId),
                        model.UserId,
                        async options => model.GuildId != null
                            ? await _client.GetMemberAsync(model.GuildId.Value, model.UserId, options).ConfigureAwait(false) 
                                ?? await _client.GetUserAsync(model.UserId, options).ConfigureAwait(false)
                            : await _client.GetUserAsync(model.UserId, options).ConfigureAwait(false)),
                    reaction ?? Optional<ReactionData>.Empty,
                    model.Emoji.ToEmoji()));
        }
    }
}
