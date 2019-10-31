using System;
using System.Threading.Tasks;
using Disqord.Models;
using Disqord.Models.Dispatches;

namespace Disqord
{
    internal sealed partial class DiscordClientState
    {
        public Task HandleGuildSyncAsync(PayloadModel payload)
        {
            var model = Serializer.ToObject<GuildSyncModel>(payload.D);
            var guild = GetGuild(model.Id);
            guild?.Update(model);
            guild.SyncTcs.SetResult(true);
            GC.Collect();
            return Task.CompletedTask;
        }
    }
}
