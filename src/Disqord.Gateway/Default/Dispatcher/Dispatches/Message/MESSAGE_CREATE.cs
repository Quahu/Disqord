using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Models;
using Qommon.Collections.Synchronized;

namespace Disqord.Gateway.Default.Dispatcher;

public class MessageCreateDispatchHandler : DispatchHandler<MessageJsonModel, MessageReceivedEventArgs>
{
    public override ValueTask<MessageReceivedEventArgs?> HandleDispatchAsync(IShard shard, MessageJsonModel model)
    {
        CachedMember? author = null;
        IGatewayMessage? message = null;
        if (model.GuildId.HasValue && !model.WebhookId.HasValue && model.Member.HasValue
            && Client.CacheProvider.TryGetUsers(out var userCache)
            && Client.CacheProvider.TryGetMembers(model.GuildId.Value, out var memberCache))
        {
            model.Member.Value.User = model.Author;
            author = Dispatcher.GetOrAddMember(userCache, memberCache, model.GuildId.Value, model.Member.Value);
            foreach (var userModel in model.Mentions)
            {
                var memberModel = userModel["member"]?.ToType<MemberJsonModel>();
                if (memberModel == null)
                    continue;

                memberModel.User = userModel;
                Dispatcher.GetOrAddMember(userCache, memberCache, model.GuildId.Value, memberModel);
            }

            if (CacheProvider.TryGetMessages(model.ChannelId, out var messageCache)
                && model.Type is UserMessageType.Default or UserMessageType.Reply or UserMessageType.SlashCommand or UserMessageType.ThreadStarterMessage or UserMessageType.ContextMenuCommand)
            {
                message = new CachedUserMessage(Client, author, model);
                messageCache.Add(model.Id, (message as CachedUserMessage)!);
            }
        }

        message ??= TransientGatewayMessage.Create(Client, model);

        CachedMessageGuildChannel? channel = null;
        if (model.GuildId.HasValue && CacheProvider.TryGetChannels(model.GuildId.Value, out var channelCache))
        {
            channel = channelCache.GetValueOrDefault(model.ChannelId) as CachedMessageGuildChannel;
            if (channel != null)
                channel.LastMessageId = model.Id;
        }

        var e = new MessageReceivedEventArgs(message, channel, author);
        return new(e);
    }
}
