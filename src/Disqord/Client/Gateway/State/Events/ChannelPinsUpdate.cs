using System;
using System.Threading.Tasks;
using Disqord.Events;
using Disqord.Logging;
using Disqord.Models;
using Disqord.Models.Dispatches;

namespace Disqord
{
    internal sealed partial class DiscordClientState
    {
        public Task HandleChannelPinsUpdateAsync(PayloadModel payload)
        {
            var model = Serializer.ToObject<ChannelPinsUpdateModel>(payload.D);
            DateTimeOffset? oldLastPinTimestamp;
            ICachedMessageChannel channel;
            if (model.GuildId != null)
            {
                var textChannel = GetGuild(model.GuildId.Value).GetTextChannel(model.ChannelId);
                oldLastPinTimestamp = textChannel.LastPinTimestamp;
                textChannel.LastPinTimestamp = model.LastPinTimestamp;
                channel = textChannel;
            }
            else
            {
                var privateChannel = GetPrivateChannel(model.ChannelId);
                if (privateChannel == null)
                {
                    Log(LogMessageSeverity.Warning, $"Discarding a channel pins update for the uncached private channel: {model.ChannelId}.");
                    return Task.CompletedTask;
                }

                oldLastPinTimestamp = privateChannel.LastPinTimestamp;
                privateChannel.LastPinTimestamp = model.LastPinTimestamp;
                channel = privateChannel;
            }

            return _client._channelPinsUpdated.InvokeAsync(new ChannelPinsUpdatedEventArgs(channel, oldLastPinTimestamp));
        }
    }
}
