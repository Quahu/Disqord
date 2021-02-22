using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Collections;
using Disqord.Gateway.Api.Models;
using Microsoft.Extensions.Logging;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class ReadyHandler : Handler<ReadyJsonModel, ReadyEventArgs>
    {
        public ICurrentUser CurrentUser => _user;

        public ISet<Snowflake> UnavailableGuildIds => _guildIds;

        private CachedCurrentUser _user;
        private SortedSet<Snowflake> _guildIds;

        public override async Task<ReadyEventArgs> HandleDispatchAsync(ReadyJsonModel model)
        {
            CacheProvider.Reset();

            // The shared user for the bot is always going to be referenced.
            var sharedUser = new CachedSharedUser(Client, model.User);
            if (CacheProvider.TryGetUsers(out var sharedUserCache))
            {
                sharedUserCache.Add(sharedUser.Id, sharedUser);
            }

            _user = new CachedCurrentUser(sharedUser, model.User);
            var guildIds = model.Guilds.ToReadOnlyList(x => x.Id);
            _guildIds = new SortedSet<Snowflake>(guildIds);
            Logger.LogInformation("Identified as {0} with {1} guilds.", _user.Tag, guildIds.Count);
            return new ReadyEventArgs(_user, guildIds);
        }
    }
}
