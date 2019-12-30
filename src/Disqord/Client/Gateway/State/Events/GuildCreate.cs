using System.Threading.Tasks;
using Disqord.Events;
using Disqord.Logging;
using Disqord.Models;

namespace Disqord
{
    internal sealed partial class DiscordClientState
    {
        public Task HandleGuildCreateAsync(PayloadModel payload)
        {
            var model = Serializer.ToObject<WebSocketGuildModel>(payload.D);
            var guild = _guilds.AddOrUpdate(model.Id,
                _ => new CachedGuild(_client, model),
                (_, oldValue) =>
                {
                    oldValue.Update(model);
                    return oldValue;
                });

            if (model.Unavailable.HasValue)
            {
                _client.GetGateway(model.Id).Log(LogMessageSeverity.Information, $"Guild '{guild}' ({guild.Id}) became available.");
                return _client._guildAvailable.InvokeAsync(new GuildAvailableEventArgs(guild));
            }
            else
            {
                if (guild.IsLarge)
                    _ = _client.GetGateway(guild.Id).SendRequestOfflineMembersAsync(guild.Id);

                _client.GetGateway(model.Id).Log(LogMessageSeverity.Information, $"Joined guild '{guild}' ({guild.Id}).");
                return _client._joinedGuild.InvokeAsync(new JoinedGuildEventArgs(guild));
            }
        }
    }
}
