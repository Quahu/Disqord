using System.Threading.Tasks;
using Disqord.Events;
using Disqord.Models;
using Disqord.Models.Dispatches;

namespace Disqord
{
    internal sealed partial class DiscordClientState
    {
        public Task HandleGuildMemberRemoveAsync(PayloadModel payload)
        {
            var model = payload.D.ToType<GuildMemberRemoveModel>();
            var guild = GetGuild(model.GuildId);

            if (guild == null)
                return Task.CompletedTask;

            var user = guild.TryRemoveMember(model.User.Id, out var member)
                ? (CachedUser) member
                : new CachedUnknownUser(_client, model.User);

            return _client._memberLeft.InvokeAsync(new MemberLeftEventArgs(guild, user));
        }
    }
}
