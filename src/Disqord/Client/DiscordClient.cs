using System.Collections.Generic;
using Disqord.Collections;
using Disqord.Models;
using Disqord.Rest;
using Disqord.Serialization.Json;
using Disqord.WebSocket;
using Qommon.Collections;

namespace Disqord
{
    public partial class DiscordClient : DiscordClientBase
    {
        /// <summary>
        ///     Gets the size of message caches per-channel.
        /// </summary>
        public int MessageCacheSize { get; }

        /// <summary>
        ///     Gets the global user cache for this <see cref="DiscordClient"/>.
        /// </summary>
        public IReadOnlyDictionary<Snowflake, CachedUser> Users { get; }

        /// <summary>
        ///     Gets the global private channel cache for this <see cref="DiscordClient"/>.
        /// </summary>
        public IReadOnlyDictionary<Snowflake, CachedPrivateChannel> PrivateChannels { get; }

        /// <summary>
        ///     Gets the global DM channel cache for this <see cref="DiscordClient"/>.
        /// </summary>
        public IReadOnlyDictionary<Snowflake, CachedDmChannel> DmChannels { get; }

        /// <summary>
        ///     Gets the global group DM channel cache for this <see cref="DiscordClient"/>.
        /// </summary>
        public IReadOnlyDictionary<Snowflake, CachedGroupChannel> GroupChannels { get; }

        /// <summary>
        ///     Gets the global guild cache for this <see cref="DiscordClient"/>.
        /// </summary>
        public IReadOnlyDictionary<Snowflake, CachedGuild> Guilds { get; }

        internal override RestDiscordClient RestClient { get; }

        internal IJsonSerializer Serializer => RestClient.Serializer;

        internal readonly LockedDictionary<Snowflake, CachedSharedUser> _users = new LockedDictionary<Snowflake, CachedSharedUser>(1);

        internal readonly LockedDictionary<Snowflake, CachedPrivateChannel> _privateChannels = new LockedDictionary<Snowflake, CachedPrivateChannel>();

        internal readonly LockedDictionary<Snowflake, CachedGuild> _guilds = new LockedDictionary<Snowflake, CachedGuild>();

        internal string Token => RestClient.ApiClient.Token;

        internal bool IsBot => TokenType == TokenType.Bot;

        public DiscordClient(TokenType tokenType, string token, DiscordClientConfiguration configuration = null) : this(configuration ?? DiscordClientConfiguration.Default)
        {
            RestClient = new RestDiscordClient(tokenType, token);
        }

        public DiscordClient(RestDiscordClient restClient, DiscordClientConfiguration configuration = null) : this(configuration ?? DiscordClientConfiguration.Default)
        {
            RestClient = restClient;
        }

        private DiscordClient(DiscordClientConfiguration configuration)
        {
            SetStatus(configuration.Status);
            SetActivity(configuration.Activity);
            MessageCacheSize = configuration.MessageCacheSize;
            _guildSubscriptions = configuration.GuildSubscriptions;
            Users = new ReadOnlyUpcastingDictionary<Snowflake, CachedSharedUser, CachedUser>(_users);
            PrivateChannels = new ReadOnlyDictionary<Snowflake, CachedPrivateChannel>(_privateChannels);
            DmChannels = new ReadOnlyOfTypeDictionary<Snowflake, CachedPrivateChannel, CachedDmChannel>(_privateChannels);
            GroupChannels = new ReadOnlyOfTypeDictionary<Snowflake, CachedPrivateChannel, CachedGroupChannel>(_privateChannels);
            Guilds = new ReadOnlyDictionary<Snowflake, CachedGuild>(_guilds);
            _ws = new WebSocketClient();
            _ws.MessageReceived += WebSocketMessageReceivedAsync;
            _ws.Closed += WebSocketClosedAsync;
        }

        /// <summary>
        ///     Looks up the user cache for the <see cref="CachedUser"/> with the given id.
        /// </summary>
        /// <param name="id"> The id of the user. </param>
        /// <returns>
        ///     The found <see cref="CachedUser"/> or <see langword="null"/>.
        /// </returns>
        public CachedUser GetUser(Snowflake id)
            => _users.GetValueOrDefault(id);

        public CachedPrivateChannel GetPrivateChannel(Snowflake id)
            => _privateChannels.GetValueOrDefault(id);

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

        public CachedChannel GetChannel(Snowflake id)
        {
            CachedChannel channel;
            if ((channel = GetPrivateChannel(id)) != null)
                return channel;

            return GetGuildChannel(id);
        }

        internal CachedSharedUser GetOrAddSharedUser(UserModel model)
        {
            var user = _users.GetOrAdd(model.Id, _ => new CachedSharedUser(this, model));
            user.References++;
            return user;
        }

        internal CachedUser GetSharedOrUnknownUser(UserModel model)
        {
            var user = GetUser(model.Id);
            if (user != null)
                return user;

            return new CachedUnknownUser(this, model);
        }

        internal CachedMember CreateMember(CachedGuild guild, MemberModel memberModel, UserModel userModel)
            => new CachedMember(GetOrAddSharedUser(userModel), guild, memberModel);

        internal CachedMember AddOrUpdateMember(CachedGuild guild, MemberModel memberModel, UserModel usermodel, bool sync = false)
            => guild.AddOrUpdateMember(usermodel.Id, _ => CreateMember(guild, memberModel, usermodel), (_, x) =>
            {
                x.Update(memberModel);
                x.Update(usermodel);
                return x;
            }, sync);

        internal CachedMember GetOrAddMember(CachedGuild guild, MemberModel memberModel, UserModel userModel, bool sync = false)
            => guild.GetOrAddMember(userModel.Id, _ => CreateMember(guild, memberModel, userModel), sync);

        internal CachedMember AddMember(CachedGuild guild, MemberModel memberModel, UserModel usermodel, bool sync = false)
        {
            var member = new CachedMember(GetOrAddSharedUser(usermodel), guild, memberModel);
            guild.TryAddMember(usermodel.Id, member, sync);
            return member;
        }
    }
}
