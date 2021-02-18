using System;
using System.Threading.Tasks;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class GuildMembersChunkHandler : Handler<GuildMembersChunkJsonModel, EventArgs>
    {
        public override async Task<EventArgs> HandleDispatchAsync(GuildMembersChunkJsonModel model)
        {

            return null;
        }
    }
}
