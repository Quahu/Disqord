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
                if (!_guilds.TryGetValue(model.Id, out var guild))
                {
                    Log(LogMessageSeverity.Information, $"Guild {model.Id} is uncached and became unavailable.");
                    return Task.CompletedTask;
                }

                // TODO set unavailable or something
                Log(LogMessageSeverity.Information, $"Guild '{guild}' ({guild.Id}) became unavailable.");
                return _client._guildUnavailable.InvokeAsync(new GuildUnavailableEventArgs(guild));
            }
            else
            {
                if (!_guilds.TryRemove(model.Id, out var guild))
                {
                    // This should only ever be the case for user tokens and lurkable guilds?
                    Log(LogMessageSeverity.Information, $"Left uncached guild {model.Id}.");
                    return Task.CompletedTask;
                }

                foreach (var member in guild.Members.Values)
                    member.SharedUser.References--;

                Log(LogMessageSeverity.Information, $"Left guild '{guild}' ({guild.Id}).");
                return _client._leftGuild.InvokeAsync(new LeftGuildEventArgs(guild));
            }
        }
    }
}
