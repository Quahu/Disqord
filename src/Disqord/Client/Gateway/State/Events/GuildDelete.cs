using System.Threading.Tasks;
using Disqord.Events;
using Disqord.Logging;
using Disqord.Models;

namespace Disqord
{
    internal sealed partial class DiscordClientState
    {
        public Task HandleGuildDeleteAsync(PayloadModel payload)
        {
            var model = Serializer.ToObject<WebSocketGuildModel>(payload.D);
            if (model.Unavailable.HasValue)
            {
                _guilds.TryGetValue(model.Id, out var guild);
                // TODO set unavailable or something
                Log(LogMessageSeverity.Information, $"Guild '{guild}' ({guild.Id}) became unavailable.");
                return _client._guildUnavailable.InvokeAsync(new GuildUnavailableEventArgs(guild));
            }
            else
            {
                _guilds.TryRemove(model.Id, out var guild);
                foreach (var member in guild.Members.Values)
                    member.SharedUser.References--;

                Log(LogMessageSeverity.Information, $"Left guild '{guild}' ({guild.Id}).");
                return _client._leftGuild.InvokeAsync(new LeftGuildEventArgs(guild));
            }
        }
    }
}
