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
                _currentUser = new CachedCurrentUser(sharedUser, model.User, model.Relationships?.Length ?? 0, model.Notes?.Count ?? 0);
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

            if (!_client.IsBot)
            {
                for (var i = 0; i < model.Guilds.Length; i++)
                {
                    var guildModel = model.Guilds[i];
                    if (guildModel.Unavailable.HasValue && guildModel.Unavailable.Value)
                    {
                        Log(LogMessageSeverity.Information, $"Guild {guildModel.Id} is unavailable.");
                        continue;
                    }

                    _guilds.AddOrUpdate(guildModel.Id, _ => new CachedGuild(_client, guildModel), (_, old) =>
                    {
                        old.Update(guildModel);
                        return old;
                    });
                }

                if (model.Guilds.Length != _guilds.Count)
                {
                    foreach (var key in _guilds.Keys)
                    {
                        var found = false;
                        for (var i = 0; i < model.Guilds.Length; i++)
                        {
                            if (key == model.Guilds[i].Id)
                            {
                                found = true;
                                break;
                            }
                        }

                        if (!found)
                            _guilds.TryRemove(key, out _);
                    }
                }

                _currentUser.Update(model.Relationships);

                _currentUser.Update(model.Notes);

                for (var i = 0; i < model.PrivateChannels.Length; i++)
                {
                    var channelModel = model.PrivateChannels[i];
                    _privateChannels.AddOrUpdate(channelModel.Id, _ => CachedPrivateChannel.Create(_client, channelModel), (_, old) =>
                    {
                        old.Update(channelModel);
                        return old;
                    });
                }

                if (model.PrivateChannels.Length != _privateChannels.Count)
                {
                    foreach (var key in _privateChannels.Keys)
                    {
                        var found = false;
                        for (var i = 0; i < model.PrivateChannels.Length; i++)
                        {
                            if (key == model.PrivateChannels[i].Id)
                            {
                                found = true;
                                break;
                            }
                        }

                        if (!found)
                            _privateChannels.TryRemove(key, out _);
                    }
                }

                return _client.GetGateway(0).SendGuildSyncAsync(_guilds.Keys.Select(x => x.RawValue));
            }

            return Task.CompletedTask;
        }
    }
}
