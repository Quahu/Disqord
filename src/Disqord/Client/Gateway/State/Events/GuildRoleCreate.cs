using System.Threading.Tasks;
using Disqord.Events;
using Disqord.Models;
using Disqord.Models.Dispatches;

namespace Disqord
{
    internal sealed partial class DiscordClientState
    {
        public Task HandleGuildRoleCreateAsync(PayloadModel payload)
        {
            var model = Serializer.ToObject<GuildRoleCreateModel>(payload.D);
            var guild = GetGuild(model.GuildId);
            var role = guild._roles.AddOrUpdate(model.Role.Id, _ => new CachedRole(guild, model.Role), (_, old) =>
            {
                old.Update(model.Role);
                return old;
            });

            return _client._roleCreated.InvokeAsync(new RoleCreatedEventArgs(role));
        }
    }
}
