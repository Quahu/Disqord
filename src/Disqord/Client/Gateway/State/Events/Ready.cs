using System.Linq;
using System.Threading.Tasks;
using Disqord.Logging;
using Disqord.Models.Dispatches;
using Disqord.Rest;

namespace Disqord
{
    internal sealed partial class DiscordClientState
    {
        public Task HandleReadyAsync(ReadyModel model)
        {
            if (_currentUser == null)
            {
                _client.RestClient.CurrentUser.Value = new RestCurrentUser(_client.RestClient, model.User);
                var sharedUser = new CachedSharedUser(_client, model.User);
                _currentUser = new CachedCurrentUser(sharedUser, model.User);
                sharedUser.References++;
                _users.TryAdd(model.User.Id, _currentUser.SharedUser);
            }
            else
            {
                _client.RestClient.CurrentUser.Value.Update(model.User);
                _currentUser.Update(model.User);
            }

            // TODO: this won't work for the sharder
            //// TODO: more, more, more stale checking
            //// I can't remember what I was supposed to be checking though
            //foreach (var guild in _guilds.Values)
            //{
            //    if (_client.IsBot)
            //    {
            //        if (guild.IsLarge)
            //        {
            //            guild.ChunksExpected = (int) Math.Ceiling(guild.MemberCount / 1000.0);
            //            guild.ChunkTcs = new TaskCompletionSource<bool>();
            //        }
            //    }
            //    else
            //    {
            //        guild.SyncTcs = new TaskCompletionSource<bool>();
            //    }

            //    var found = false;
            //    for (var i = 0; i < model.Guilds.Length; i++)
            //    {
            //        if (guild.Id == model.Guilds[i].Id)
            //        {
            //            found = true;
            //            break;
            //        }
            //    }

            //    if (!found)
            //        _guilds.TryRemove(guild.Id, out _);
            //}

            return Task.CompletedTask;
        }
    }
}
