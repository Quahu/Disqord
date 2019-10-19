using System.Collections.Concurrent;
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

        internal readonly ConcurrentDictionary<Snowflake, CachedSharedUser> _users = Extensions.CreateConcurrentDictionary<Snowflake, CachedSharedUser>(1);

        internal readonly ConcurrentDictionary<Snowflake, CachedPrivateChannel> _privateChannels = Extensions.CreateConcurrentDictionary<Snowflake, CachedPrivateChannel>(0);

        internal readonly ConcurrentDictionary<Snowflake, CachedGuild> _guilds = Extensions.CreateConcurrentDictionary<Snowflake, CachedGuild>(0);

        internal string Token => RestClient.ApiClient.Token;

        internal readonly bool IsBot;

        public DiscordClient(TokenType tokenType, string token, DiscordClientConfiguration configuration = null) : this(configuration ?? DiscordClientConfiguration.Default)
        {
            RestClient = new RestDiscordClient(tokenType, token);
            IsBot = RestClient.TokenType == TokenType.Bot;
        }

        public DiscordClient(RestDiscordClient restClient, DiscordClientConfiguration configuration = null) : this(configuration ?? DiscordClientConfiguration.Default)
        {
            RestClient = restClient;
            IsBot = RestClient.TokenType == TokenType.Bot;
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
            => _users.TryGetValue(id, out var user) ? user : null;

        public CachedPrivateChannel GetPrivateChannel(Snowflake id)
            => _privateChannels.TryGetValue(id, out var channel) ? channel : null;

        public CachedGuild GetGuild(Snowflake id)
            => _guilds.TryGetValue(id, out var guild) ? guild : null;

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

        internal CachedSharedUser CreateSharedUser(UserModel model)
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

        internal CachedMember GetOrCreateMember(CachedGuild guild, MemberModel memberModel, UserModel userModel, bool sync = false)
            => guild.GetOrAddMember(userModel.Id, _ => new CachedMember(this, memberModel, guild, CreateSharedUser(userModel)), sync);

        internal CachedMember CreateMember(CachedGuild guild, MemberModel memberModel, UserModel usermodel, bool sync = false)
        {
            var member = new CachedMember(this, memberModel, guild, CreateSharedUser(usermodel));
            guild.TryAddMember(usermodel.Id, member, sync);
            return member;
        }
    }
}
