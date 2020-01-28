using System.Threading.Tasks;
using Disqord.Collections;
using Disqord.Events;
using Disqord.Logging;
using Disqord.Models;
using Disqord.Models.Dispatches;

namespace Disqord
{
    internal sealed partial class DiscordClientState
    {
        public Task HandleMessageDeleteBulkAsync(PayloadModel payload)
        {
            var model = Serializer.ToObject<MessageDeleteBulkModel>(payload.D);
            if (model.GuildId == null)
            {
                Log(LogMessageSeverity.Error, $"MessageDeleteBulk contains a null guild_id. Channel id: {model.ChannelId}.");
                return Task.CompletedTask;
            }

            var guild = GetGuild(model.GuildId.Value);
            var channel = guild.GetTextChannel(model.ChannelId);
            var messages = new SnowflakeOptional<CachedUserMessage>[model.Ids.Length];
            for (var i = 0; i < model.Ids.Length; i++)
            {
                var id = model.Ids[i];
                CachedUserMessage message = null;
                _messageCache?.TryRemoveMessage(channel.Id, id, out message);
                messages[i] = new SnowflakeOptional<CachedUserMessage>(message, id);
            }

            return _client._messagesBulkDeleted.InvokeAsync(new MessagesBulkDeletedEventArgs(channel, messages.ReadOnly()));
        }
    }
}
