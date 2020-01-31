using System;
using System.Collections.Generic;
using Disqord.Collections;
using Disqord.Logging;
using Disqord.Models;
using Disqord.Serialization.Json;

namespace Disqord
{
    internal sealed partial class DiscordClientState
    {
        public ReadOnlyDictionary<Snowflake, CachedGuild> Guilds { get; }
        public ReadOnlyUpcastingDictionary<Snowflake, CachedSharedUser, CachedUser> Users { get; }
        public ReadOnlyDictionary<Snowflake, CachedPrivateChannel> PrivateChannels { get; }
        public ReadOnlyOfTypeDictionary<Snowflake, CachedPrivateChannel, CachedDmChannel> DmChannels { get; }
        public ReadOnlyOfTypeDictionary<Snowflake, CachedPrivateChannel, CachedGroupChannel> GroupChannels { get; }

        internal CachedCurrentUser _currentUser;
        internal readonly MessageCache _messageCache;
        internal readonly LockedDictionary<Snowflake, CachedGuild> _guilds;
        internal readonly LockedDictionary<Snowflake, CachedSharedUser> _users;
        internal readonly LockedDictionary<Snowflake, CachedPrivateChannel> _privateChannels;

        internal IJsonSerializer Serializer => _client.Serializer;
        internal ILogger Logger => _client.Logger;

        internal DiscordClientBase _client;

        public DiscordClientState(DiscordClientBase client, MessageCache messageCache)
        {
            _client = client;
            _messageCache = messageCache ?? DummyMessageCache.Instance;

            _guilds = new LockedDictionary<Snowflake, CachedGuild>();
            _users = new LockedDictionary<Snowflake, CachedSharedUser>();
            _privateChannels = new LockedDictionary<Snowflake, CachedPrivateChannel>();

            Guilds = new ReadOnlyDictionary<Snowflake, CachedGuild>(_guilds);
            Users = new ReadOnlyUpcastingDictionary<Snowflake, CachedSharedUser, CachedUser>(_users);
            PrivateChannels = new ReadOnlyDictionary<Snowflake, CachedPrivateChannel>(_privateChannels);
            DmChannels = new ReadOnlyOfTypeDictionary<Snowflake, CachedPrivateChannel, CachedDmChannel>(_privateChannels);
            GroupChannels = new ReadOnlyOfTypeDictionary<Snowflake, CachedPrivateChannel, CachedGroupChannel>(_privateChannels);
        }

        internal void Log(LogMessageSeverity severity, string message, Exception exception = null)
            => _client.Log(severity, message, exception);

        public CachedUserMessage GetMessage(Snowflake channelId, Snowflake messageId)
        {
            if (_messageCache == null)
                return null;

            return _messageCache.TryGetMessage(channelId, messageId, out var message) ? message : null;
        }

        public IReadOnlyList<CachedUserMessage> GetMessages(Snowflake channelId)
        {
            if (_messageCache == null)
                return null;

            return _messageCache.TryGetMessages(channelId, out var messages)
                ? messages.ToReadOnlyList()
                : ReadOnlyList<CachedUserMessage>.Empty;
        }

        public CachedGuild GetGuild(Snowflake id)
            => _guilds.GetValueOrDefault(id);

        public CachedGuildChannel GetGuildChannel(Snowflake id)
        {
            foreach (var guild in _guilds.Values)
            {
                if (guild.Channels.TryGetValue(id, out var channel))
                    return channel;
            }

            return null;
        }

        public CachedUser GetUser(Snowflake id)
            => _users.GetValueOrDefault(id);

        public CachedPrivateChannel GetPrivateChannel(Snowflake id)
            => _privateChannels.GetValueOrDefault(id);

        public CachedChannel GetChannel(Snowflake id)
        {
            CachedChannel channel;
            if ((channel = GetPrivateChannel(id)) != null)
                return channel;

            return GetGuildChannel(id);
        }

        public CachedSharedUser GetOrAddSharedUser(UserModel model)
        {
            var user = _users.GetOrAdd(model.Id, _ => new CachedSharedUser(_client, model));
            user.References++;
            return user;
        }

        public CachedUser GetSharedOrUnknownUser(UserModel model)
        {
            var user = GetUser(model.Id);
            if (user != null)
                return user;

            return new CachedUnknownUser(_client, model);
        }

        public CachedMember CreateMember(CachedGuild guild, MemberModel memberModel, UserModel userModel)
            => new CachedMember(GetOrAddSharedUser(userModel), guild, memberModel);

        public CachedMember AddOrUpdateMember(CachedGuild guild, MemberModel memberModel, UserModel usermodel, bool sync = false)
            => guild.AddOrUpdateMember(usermodel.Id, _ => CreateMember(guild, memberModel, usermodel), (_, x) =>
            {
                x.Update(memberModel);
                x.Update(usermodel);
                return x;
            }, sync);

        public CachedMember GetOrAddMember(CachedGuild guild, MemberModel memberModel, UserModel userModel, bool sync = false)
            => guild.GetOrAddMember(userModel.Id, _ => CreateMember(guild, memberModel, userModel), sync);

        public CachedMember AddMember(CachedGuild guild, MemberModel memberModel, UserModel usermodel, bool sync = false)
        {
            var member = new CachedMember(GetOrAddSharedUser(usermodel), guild, memberModel);
            guild.TryAddMember(usermodel.Id, member, sync);
            return member;
        }

        public void Reset()
        {
            _currentUser = null;
            _messageCache.Clear();
            _guilds.Clear();
            _users.Clear();
            _privateChannels.Clear();
        }
    }
}
