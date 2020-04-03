using System;
using System.Threading.Tasks;
using Disqord.Logging;
using Disqord.Models;
using Disqord.Models.Dispatches;

namespace Disqord
{
    internal sealed partial class DiscordClientState
    {
        public Task HandleGuildMembersChunkAsync(PayloadModel payload)
        {
            var model = Serializer.ToObject<GuildMembersChunkModel>(payload.D);
            var guild = GetGuild(model.GuildId);
            Log(LogMessageSeverity.Debug, $"Received a member chunk with {model.Members.Length} members for {guild} ({guild.Id}).");
            if (--guild.ChunksExpected == 0)
            {
                guild.ChunkTcs.SetResult(true);
                GC.Collect();
            }

            guild.Update(model);
            return Task.CompletedTask;
        }
    }
}
