using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Collections;
using Disqord.Models;
using Disqord.Models.Dispatches;
using Qommon.Collections;

namespace Disqord
{
    public sealed partial class CachedGuild : CachedSnowflakeEntity, IGuild
    {
        public string Name { get; private set; }

        public string IconHash { get; private set; }

        public string SplashHash { get; private set; }

        public Snowflake OwnerId { get; private set; }

        public CachedMember Owner => GetMember(OwnerId);

        public string VoiceRegionId { get; private set; }

        public Snowflake? AfkChannelId { get; private set; }

        public int AfkTimeout { get; private set; }

        public bool IsEmbedEnabled { get; private set; }

        public Snowflake? EmbedChannelId { get; private set; }

        public VerificationLevel VerificationLevel { get; private set; }

        public DefaultNotificationLevel DefaultNotificationLevel { get; private set; }

        public ContentFilterLevel ContentFilterLevel { get; private set; }

        public IReadOnlyList<CachedGuildEmoji> Emojis { get; private set; }

        public IReadOnlyList<string> Features { get; private set; }

        public MfaLevel MfaLevel { get; private set; }

        public Snowflake? ApplicationId { get; private set; }

        public bool IsWidgetEnabled { get; private set; }

        public Snowflake? WidgetChannelId { get; private set; }

        public CachedGuildChannel WidgetChannel
        {
            get
            {
                var widgetChannelId = WidgetChannelId;
                return widgetChannelId != null
                    ? GetChannel(widgetChannelId.Value)
                    : null;
            }
        }

        public Snowflake? SystemChannelId { get; private set; }

        public CachedTextChannel SystemChannel
        {
            get
            {
                var systemChannelId = SystemChannelId;
                return systemChannelId != null
                    ? GetTextChannel(systemChannelId.Value)
                    : null;
            }
        }

        public bool IsLarge { get; private set; }

        public int MemberCount { get; private set; }

        public bool IsUnavailable { get; private set; }

        public CachedMember CurrentMember => Members.GetValueOrDefault(Client.CurrentUser.Id);

        public IReadOnlyDictionary<Snowflake, CachedRole> Roles { get; }

        public IReadOnlyDictionary<Snowflake, CachedGuildChannel> Channels { get; }

        public IReadOnlyDictionary<Snowflake, CachedNestedChannel> NestedChannels { get; }

        public IReadOnlyDictionary<Snowflake, CachedTextChannel> TextChannels { get; }

        public IReadOnlyDictionary<Snowflake, CachedVoiceChannel> VoiceChannels { get; }

        public IReadOnlyDictionary<Snowflake, CachedCategoryChannel> CategoryChannels { get; }

        public IReadOnlyDictionary<Snowflake, CachedMember> Members { get; }

        public bool IsSynced
        {
            get
            {
                // TODO: exception message
                if (Client.IsBot)
                    throw new NotSupportedException();

                return SyncTcs.Task.IsCompleted;
            }
        }

        public bool IsChunked
        {
            get
            {
                // TODO: exception message
                if (!Client.IsBot)
                    throw new NotSupportedException();

                return ChunkTcs == null || ChunkTcs.Task.IsCompleted;
            }
        }

        public BoostTier BoostTier { get; private set; }

        public int BoostingMemberCount { get; private set; }

        public int MaxPresenceCount { get; private set; }

        public int MaxMemberCount { get; private set; }

        public string VanityUrlCode { get; private set; }

        public string Description { get; private set; }

        public string BannerHash { get; private set; }

        public CultureInfo PreferredLocale { get; private set; }

        internal TaskCompletionSource<bool> SyncTcs;

        internal int ChunksExpected;

        internal TaskCompletionSource<bool> ChunkTcs;

        internal readonly ConcurrentDictionary<Snowflake, CachedRole> _roles;

        internal readonly ConcurrentDictionary<Snowflake, CachedGuildChannel> _channels;

        internal readonly ConcurrentDictionary<Snowflake, CachedMember> _members;

        IReadOnlyDictionary<Snowflake, IRole> IGuild.Roles => new ReadOnlyUpcastingDictionary<Snowflake, CachedRole, IRole>(Roles);
        IReadOnlyList<IGuildEmoji> IGuild.Emojis => Emojis;

        internal CachedGuild(DiscordClient client, WebSocketGuildModel model) : base(client, model.Id)
        {
            _roles = Extensions.CreateConcurrentDictionary<Snowflake, CachedRole>(model.Roles.Value.Length);
            _channels = Extensions.CreateConcurrentDictionary<Snowflake, CachedGuildChannel>(model.Channels.Length);
            _members = Extensions.CreateConcurrentDictionary<Snowflake, CachedMember>(model.Members.Length);
            Roles = new ReadOnlyDictionary<Snowflake, CachedRole>(_roles);
            Channels = new ReadOnlyDictionary<Snowflake, CachedGuildChannel>(_channels);
            Members = new ReadOnlyDictionary<Snowflake, CachedMember>(_members);
            NestedChannels = new ReadOnlyOfTypeDictionary<Snowflake, CachedGuildChannel, CachedNestedChannel>(_channels);
            TextChannels = new ReadOnlyOfTypeDictionary<Snowflake, CachedGuildChannel, CachedTextChannel>(_channels);
            VoiceChannels = new ReadOnlyOfTypeDictionary<Snowflake, CachedGuildChannel, CachedVoiceChannel>(_channels);
            CategoryChannels = new ReadOnlyOfTypeDictionary<Snowflake, CachedGuildChannel, CachedCategoryChannel>(_channels);

            Update(model);
            if (client.IsBot && IsLarge)
            {
                ChunksExpected = (int) Math.Ceiling(model.MemberCount / 1000.0);
                ChunkTcs = new TaskCompletionSource<bool>();
            }
            else if (!client.IsBot)
            {
                SyncTcs = new TaskCompletionSource<bool>();
            }
        }

        internal void Update(GuildMembersChunkModel model)
        {
            for (var i = 0; i < model.Members.Length; i++)
            {
                var memberModel = model.Members[i];
                _members.AddOrUpdate(memberModel.User.Id,
                    _ => Client.GetOrCreateMember(this, memberModel, memberModel.User, true),
                    (_, x) =>
                    {
                        x.Update(memberModel);
                        return x;
                    });
            }

            if (model.Presences != null)
                Update(model.Presences);
        }

        internal void Update(GuildSyncModel model)
        {
            for (var i = 0; i < model.Members.Length; i++)
            {
                var memberModel = model.Members[i];
                _members.AddOrUpdate(memberModel.User.Id,
                    _ => Client.GetOrCreateMember(this, memberModel, memberModel.User, true),
                    (_, x) =>
                    {
                        x.Update(memberModel);
                        return x;
                    });
            }

            if (_members.Count != model.Members.Length)
            {
                foreach (var key in _members.Keys.Except(model.Members.Select(x => new Snowflake(x.User.Id))))
                    _members.TryRemove(key, out _);
            }

            Update(model.Presences);
        }

        internal void Update(PresenceUpdateModel[] models)
        {
            for (var i = 0; i < models.Length; i++)
            {
                var presenceModel = models[i];
                GetMember(presenceModel.User.Id).Update(presenceModel);
            }
        }

        internal void Update(WebSocketGuildModel model)
        {
            Update(model as GuildModel);

            _channels.Clear();
            for (var i = 0; i < model.Channels.Length; i++)
            {
                var channelModel = model.Channels[i];
                _channels.TryAdd(channelModel.Id, CachedGuildChannel.Create(Client, channelModel, this));
            }

            _members.Clear();
            for (var i = 0; i < model.Members.Length; i++)
            {
                var memberModel = model.Members[i];
                CachedMember member = null;
                if (Client.IsBot || !Client.IsBot && !_members.ContainsKey(memberModel.User.Id))
                    member = Client.GetOrCreateMember(this, memberModel, memberModel.User, true);

                if (member != null)
                {
                    var voiceState = Array.Find(model.VoiceStates, x => x.UserId == member.Id);
                    if (voiceState != null)
                        member.Update(voiceState);
                }
            }

            if (model.Presences != null)
                Update(model.Presences);

            MemberCount = model.MemberCount;
            IsLarge = model.Large;
            IsUnavailable = !model.Unavailable.HasValue || model.Unavailable.Value;
        }

        internal void Update(GuildModel model)
        {
            if (model.Name.HasValue)
                Name = model.Name.Value;

            if (model.Icon.HasValue)
                IconHash = model.Icon.Value;

            if (model.Splash.HasValue)
                SplashHash = model.Splash.Value;

            if (model.OwnerId.HasValue)
                OwnerId = model.OwnerId.Value;

            if (model.Region.HasValue)
                VoiceRegionId = model.Region.Value;

            if (model.AfkChannelId.HasValue)
                AfkChannelId = model.AfkChannelId.Value;

            if (model.AfkTimeout.HasValue)
                AfkTimeout = model.AfkTimeout.Value;

            if (model.EmbedChannelId.HasValue)
                IsEmbedEnabled = model.EmbedEnabled.Value;

            if (model.EmbedChannelId.HasValue)
                EmbedChannelId = model.EmbedChannelId.Value;

            if (model.VerificationLevel.HasValue)
                VerificationLevel = model.VerificationLevel.Value;

            if (model.DefaultMessageNotifications.HasValue)
                DefaultNotificationLevel = model.DefaultMessageNotifications.Value;

            if (model.ExplicitContentFilter.HasValue)
                ContentFilterLevel = model.ExplicitContentFilter.Value;

            if (model.Roles.HasValue)
            {
                _roles.Clear();
                for (var i = 0; i < model.Roles.Value.Length; i++)
                {
                    var roleModel = model.Roles.Value[i];
                    _roles.TryAdd(roleModel.Id, new CachedRole(Client, roleModel, this));
                }
            }

            if (model.Emojis.HasValue)
                Emojis = model.Emojis.Value.Select(x => new CachedGuildEmoji(Client, x, this)).ToImmutableArray();

            if (model.Features.HasValue)
                Features = model.Features.Value.ToImmutableArray();

            if (model.MfaLevel.HasValue)
                MfaLevel = model.MfaLevel.Value;

            if (model.ApplicationId.HasValue)
                ApplicationId = model.ApplicationId.Value;

            if (model.WidgetEnabled.HasValue)
                IsWidgetEnabled = model.WidgetEnabled.Value;

            if (model.WidgetChannelId.HasValue)
                WidgetChannelId = model.WidgetChannelId.Value;

            if (model.SystemChannelId.HasValue)
                SystemChannelId = model.SystemChannelId.Value;

            if (model.MaxPresences.HasValue)
                MaxPresenceCount = model.MaxPresences.Value ?? Discord.DEFAULT_MAX_PRESENCE_COUNT;

            if (model.MaxMembers.HasValue)
                MaxMemberCount = model.MaxMembers.Value;

            if (model.VanityUrlCode.HasValue)
                VanityUrlCode = model.VanityUrlCode.Value;

            if (model.Description.HasValue)
                Description = model.Description.Value;

            if (model.Banner.HasValue)
                BannerHash = model.Banner.Value;

            if (model.PremiumTier.HasValue)
                BoostTier = model.PremiumTier.Value;

            if (model.PremiumSubscriptionCount.HasValue && model.PremiumSubscriptionCount.Value != null)
                BoostingMemberCount = model.PremiumSubscriptionCount.Value.Value;

            if (model.PreferredLocale.HasValue)
                PreferredLocale = Discord.Internal.CreateLocale(model.PreferredLocale.Value);
        }

        internal bool TryAddMember(Snowflake id, CachedMember member, bool sync = false)
        {
            var result = _members.TryAdd(id, member);
            if (result && !sync)
                MemberCount++;
            return result;
        }

        internal bool TryRemoveMember(Snowflake id, out CachedMember member)
        {
            var result = _members.TryRemove(id, out member);
            if (result)
                MemberCount--;
            return result;
        }

        internal CachedMember AddOrUpdateMember(Snowflake id, Func<Snowflake, CachedMember> addFunc, Func<Snowflake, CachedMember, CachedMember> updateFunc, bool sync = false)
            => _members.AddOrUpdate(id, x =>
            {
                var member = addFunc(x);
                if (!sync)
                    MemberCount++;
                return member;
            }, updateFunc);

        internal CachedMember GetOrAddMember(Snowflake id, Func<Snowflake, CachedMember> func, bool sync = false)
            => _members.GetOrAdd(id, x =>
            {
                var member = func(x);
                if (!sync)
                    MemberCount++;
                return member;
            });

        internal CachedGuild Clone()
            => (CachedGuild) MemberwiseClone();

        public CachedGuildChannel GetChannel(Snowflake id)
            => _channels.GetValueOrDefault(id);

        public CachedTextChannel GetTextChannel(Snowflake id)
            => GetChannel(id) as CachedTextChannel;

        public CachedVoiceChannel GetVoiceChannel(Snowflake id)
            => GetChannel(id) as CachedVoiceChannel;

        public CachedCategoryChannel GetCategoryChannel(Snowflake id)
            => GetChannel(id) as CachedCategoryChannel;

        public CachedMember GetMember(Snowflake id)
            => _members.GetValueOrDefault(id);

        public CachedRole GetRole(Snowflake id)
            => _roles.GetValueOrDefault(id);

        public string GetIconUrl(ImageFormat format = default, int size = 2048)
            => Discord.GetGuildIconUrl(Id, IconHash, format, size);

        public string GetSplashUrl(int size = 2048)
            => Discord.GetGuildSplashUrl(Id, SplashHash, ImageFormat.Png, 2048);

        public override string ToString()
            => Name;
    }
}