using System.Threading.Tasks;
using Disqord.Events;
using Disqord.Logging;
using Disqord.Models;

namespace Disqord
{
    internal sealed partial class DiscordClientState
    {
        public Task HandleMessageUpdateAsync(PayloadModel payload)
        {
            var model = Serializer.ToObject<MessageModel>(payload.D);
            if (!model.EditedTimestamp.HasValue)
                return Task.CompletedTask;

            ICachedMessageChannel channel;
            CachedGuild guild = null;
            if (model.GuildId != null)
            {
                guild = GetGuild(model.GuildId.Value);
                channel = guild.GetTextChannel(model.ChannelId);
            }
            else
            {
                channel = GetPrivateChannel(model.ChannelId);
            }

            if (channel == null)
            {
                Log(LogMessageSeverity.Warning, $"Uncached channel in MessageUpdated. Id: {model.ChannelId}");
                return Task.CompletedTask;
            }

            var message = channel.GetMessage(model.Id);
            var before = message?.Clone();
            var isWebhook = model.WebhookId.HasValue;
            if (message == null)
            {
                CachedUser author = null;
                if (!model.Author.HasValue && !isWebhook)
                {
                    Log(LogMessageSeverity.Warning, "Unknown message and author has no value in MessageUpdated.");
                    return Task.CompletedTask;
                }
                else if (!isWebhook)
                {
                    if (guild != null)
                    {
                        if (guild.Members.TryGetValue(model.Author.Value.Id, out var member))
                            author = member;

                        else if (model.Member.HasValue)
                            author = GetOrAddMember(guild, model.Member.Value, model.Author.Value);
                    }
                    else
                    {
                        author = GetUser(model.Author.Value.Id);
                    }
                }
                else
                {
                    // TODO?
                    // (if isWebhook and no author)
                    return Task.CompletedTask;
                }

                if (author == null)
                {
                    // TODO
                    Log(LogMessageSeverity.Error, "Author is still null in MessageUpdate.");
                    return Task.CompletedTask;
                }

                message = new CachedUserMessage(channel, author, model);
            }
            else
            {
                message.Update(model);
            }

            return _client._messageUpdated.InvokeAsync(new MessageUpdatedEventArgs(channel,
                new SnowflakeOptional<CachedUserMessage>(before, model.Id),
                message));
        }
    }
}
