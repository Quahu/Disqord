using System.Threading.Tasks;
using Disqord.Events;
using Disqord.Models;
using Disqord.Models.Dispatches;

namespace Disqord
{
    internal sealed partial class DiscordClientState
    {
        public Task HandleGuildEmojisUpdateAsync(PayloadModel payload)
        {
            var model = Serializer.ToObject<GuildEmojisUpdateModel>(payload.D);
            var guild = GetGuild(model.GuildId);
            var oldEmojis = guild.Emojis;
            guild.Update(model.Emojis);

            return _client._guildEmojisUpdated.InvokeAsync(new GuildEmojisUpdatedEventArgs(guild, oldEmojis));
        }
    }
}
