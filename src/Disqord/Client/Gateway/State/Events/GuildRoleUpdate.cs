using System.Threading.Tasks;
using Disqord.Events;
using Disqord.Models;
using Disqord.Models.Dispatches;

namespace Disqord
{
    internal sealed partial class DiscordClientState
    {
        public Task HandleGuildRoleUpdateAsync(PayloadModel payload)
        {
            var model = Serializer.ToObject<GuildRoleUpdateModel>(payload.D);
            var guild = GetGuild(model.GuildId);
            CachedRole before = null;
            var after = guild._roles.AddOrUpdate(model.Role.Id,
                _ => new CachedRole(guild, model.Role),
                (_, old) =>
                {
                    before = old.Clone();
                    old.Update(model.Role);
                    return old;
                });

            return _client._roleUpdated.InvokeAsync(new RoleUpdatedEventArgs(before, after));
        }
    }
}
