using System.Threading.Tasks;
using Disqord.Events;
using Disqord.Models;
using Disqord.Models.Dispatches;

namespace Disqord
{
    internal sealed partial class DiscordClientState
    {
        public Task HandleGuildBanRemoveAsync(PayloadModel payload)
        {
            var model = payload.D.ToType<GuildBanRemoveModel>();
            var guild = GetGuild(model.GuildId);
            var user = GetSharedOrUnknownUser(model.User);

            return _client._memberUnbanned.InvokeAsync(new MemberUnbannedEventArgs(guild, user));
        }
    }
}
