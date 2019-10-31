using System.Linq;
using System.Threading.Tasks;
using Disqord.Models.Dispatches;
using Disqord.Rest;

namespace Disqord
{
    internal sealed partial class DiscordClientState
    {
        public Task HandleReadyAsync(ReadyModel model)
        {
            _client.RestClient.CurrentUser.SetValue(new RestCurrentUser(_client.RestClient, model.User));
            var sharedUser = new CachedSharedUser(_client, model.User);
            _currentUser = new CachedCurrentUser(sharedUser, model.User, model.Relationships?.Length ?? 0, model.Notes?.Count ?? 0);
            sharedUser.References++;
            _users.TryAdd(model.User.Id, _currentUser.SharedUser);

            if (!_client.IsBot)
            {
                foreach (var guildModel in model.Guilds)
                    _guilds.TryAdd(guildModel.Id, new CachedGuild(_client, guildModel));

                foreach (var note in model.Notes)
                    _currentUser.AddOrUpdateNote(note.Key, note.Value, (_, __) => note.Value);

                for (var i = 0; i < model.Relationships.Length; i++)
                {
                    var relationshipModel = model.Relationships[i];
                    var relationship = new CachedRelationship(_client, relationshipModel);
                    _currentUser.TryAddRelationship(relationship);
                }

                for (var i = 0; i < model.PrivateChannels.Length; i++)
                {
                    var channelModel = model.PrivateChannels[i];
                    var channel = CachedPrivateChannel.Create(_client, channelModel);
                    _privateChannels.TryAdd(channel.Id, channel);
                }

                return _getGateway(_client, 0).SendGuildSyncAsync(_guilds.Keys.Select(x => x.RawValue));
            }

            return Task.CompletedTask;
        }
    }
}
