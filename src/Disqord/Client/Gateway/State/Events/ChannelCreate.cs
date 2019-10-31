using System.Threading.Tasks;
using Disqord.Events;
using Disqord.Models;

namespace Disqord
{
    internal sealed partial class DiscordClientState
    {
        public Task HandleChannelCreateAsync(PayloadModel payload)
        {
            var model = Serializer.ToObject<ChannelModel>(payload.D);
            CachedChannel channel;
            if (model.GuildId != null)
            {
                var guild = GetGuild(model.GuildId.Value);
                channel = CachedGuildChannel.Create(guild, model);
                guild._channels.TryAdd(channel.Id, (CachedGuildChannel) channel);
            }
            else
            {
                channel = _privateChannels.GetOrAdd(model.Id, _ => CachedPrivateChannel.Create(_client, model));
            }

            return _client._channelCreated.InvokeAsync(new ChannelCreatedEventArgs(channel));
        }
    }
}
