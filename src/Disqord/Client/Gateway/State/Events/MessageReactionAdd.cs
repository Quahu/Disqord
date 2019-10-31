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

            return _client._reactionAdded.InvokeAsync(
                new ReactionAddedEventArgs(
                    channel,
                    new DownloadableOptionalSnowflakeEntity<CachedMessage, RestMessage>(
                        message, model.MessageId, options => _client.GetMessageAsync(channel.Id, model.MessageId, options)),
                    new DownloadableOptionalSnowflakeEntity<CachedUser, RestUser>(message?.Author, model.UserId,
                    async options => model.GuildId != null
                        ? await _client.GetMemberAsync(model.GuildId.Value, model.UserId, options).ConfigureAwait(false)
                        : await _client.GetUserAsync(model.UserId, options).ConfigureAwait(false)),
                    reaction ?? Optional<ReactionData>.Empty,
                    emoji));
        }
    }
}
