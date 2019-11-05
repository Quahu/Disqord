using System.Threading.Tasks;
using Disqord.Events;
using Disqord.Logging;
using Disqord.Models;

namespace Disqord
{
    internal sealed partial class DiscordClientState
    {
        public Task HandleMessageCreateAsync(PayloadModel payload)
        {
            var model = Serializer.ToObject<MessageModel>(payload.D);
            ICachedMessageChannel channel;
            CachedUser author = null;
            CachedGuild guild = null;
            var isWebhook = model.WebhookId.HasValue;
            if (model.GuildId != null)
            {
                guild = GetGuild(model.GuildId.Value);
                channel = guild.GetTextChannel(model.ChannelId);

                if (!isWebhook)
                    author = model.Author.HasValue && model.Member.HasValue
                        ? GetOrAddMember(guild, model.Member.Value, model.Author.Value)
                        : guild.GetMember(model.Author.Value.Id);
            }
            else
            {
                channel = GetPrivateChannel(model.ChannelId);

                if (!isWebhook)
                    author = GetUser(model.Author.Value.Id);
            }

            if (author == null && !isWebhook)
            {
                Log(LogMessageSeverity.Warning, $"Uncached author and/or guild == null in MESSAGE_CREATE.\n{payload.D}");
                return Task.CompletedTask;
            }

            var message = CachedMessage.Create(channel, author, model);
            if (message is CachedUserMessage userMessage)
                _messageCache?.TryAddMessage(userMessage);

            if (guild != null)
                ((CachedTextChannel) channel).LastMessageId = message.Id;
            else
                ((CachedPrivateChannel) channel).LastMessageId = message.Id;

            return _client._messageReceived.InvokeAsync(new MessageReceivedEventArgs(message));
        }
    }
}
