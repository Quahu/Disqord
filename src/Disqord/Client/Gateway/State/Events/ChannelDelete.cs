using System.Threading.Tasks;
using Disqord.Events;
using Disqord.Logging;
using Disqord.Models;

namespace Disqord
{
    internal sealed partial class DiscordClientState
    {
        public Task HandleChannelDeleteAsync(PayloadModel payload)
        {
            var model = Serializer.ToObject<ChannelModel>(payload.D);
            CachedChannel channel;
            if (model.GuildId != null)
            {
                var guild = GetGuild(model.GuildId.Value);
                guild._channels.TryRemove(model.Id, out var guildChannel);
                channel = guildChannel;
            }
            else
            {
                _privateChannels.TryRemove(model.Id, out var privateChannel);
                channel = privateChannel;
            }

            if (channel == null)
            {
                _client.Log(LogMessageSeverity.Warning, $"Unknown channel in ChannelDelete. Id: {model.Id}.");
                return Task.CompletedTask;
            }

            return _client._channelDeleted.InvokeAsync(new ChannelDeletedEventArgs(channel));
        }
    }
}
