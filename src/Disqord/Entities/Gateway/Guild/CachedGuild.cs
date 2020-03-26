using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Collections;
using Disqord.Models;
using Disqord.Models.Dispatches;

namespace Disqord
{
    public sealed partial class CachedGuild : CachedSnowflakeEntity, IGuild
    {
        public string Name { get; private set; }

        public string IconHash { get; private set; }

        public string SplashHash { get; private set; }

        public string DiscoverySplashHash { get; private set; }

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

        public CachedRole DefaultRole => Roles[Id];

        public IReadOnlyDictionary<Snowflake, CachedRole> Roles => _roles.ReadOnly();

        public IReadOnlyDictionary<Snowflake, CachedGuildEmoji> Emojis => _emojis.ReadOnly();

        public IReadOnlyDictionary<Snowflake, CachedGuildChannel> Channels => _channels.ReadOnly();

        public IReadOnlyDictionary<Snowflake, CachedNestedChannel> NestedChannels
            => new ReadOnlyOfTypeDictionary<Snowflake, CachedGuildChannel, CachedNestedChannel>(_channels);

        public IReadOnlyDictionary<Snowflake, CachedTextChannel> TextChannels
            => new ReadOnlyOfTypeDictionary<Snowflake, CachedGuildChannel, CachedTextChannel>(_channels);

        public IReadOnlyDictionary<Snowflake, CachedVoiceChannel> VoiceChannels
            => new ReadOnlyOfTypeDictionary<Snowflake, CachedGuildChannel, CachedVoiceChannel>(_channels);

        public IReadOnlyDictionary<Snowflake, CachedCategoryChannel> CategoryChannels
            => new ReadOnlyOfTypeDictionary<Snowflake, CachedGuildChannel, CachedCategoryChannel>(_channels);

        public IReadOnlyDictionary<Snowflake, CachedMember> Members => _members.ReadOnly();

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

        internal readonly LockedDictionary<Snowflake, CachedGuildEmoji> _emojis;

        internal readonly LockedDictionary<Snowflake, CachedRole> _roles;

        internal readonly LockedDictionary<Snowflake, CachedGuildChannel> _channels;

        internal readonly LockedDictionary<Snowflake, CachedMember> _members;

        IReadOnlyDictionary<Snowflake, IRole> IGuild.Roles => new ReadOnlyUpcastingDictionary<Snowflake, CachedRole, IRole>(Roles);
        IReadOnlyDictionary<Snowflake, IGuildEmoji> IGuild.Emojis => new ReadOnlyUpcastingDictionary<Snowflake, CachedGuildEmoji, IGuildEmoji>(Emojis);

        internal CachedGuild(DiscordClientBase client, WebSocketGuildModel model) : base(client, model.Id)
        {
            _roles = new LockedDictionary<Snowflake, CachedRole>(model.Roles.Value.Length);
            _emojis = new LockedDictionary<Snowflake, CachedGuildEmoji>(model.Emojis.Value.Length);
            _channels = new LockedDictionary<Snowflake, CachedGuildChannel>(model.Channels.Length);
            _members = new LockedDictionary<Snowflake, CachedMember>(model.MemberCount);

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
            Update(model.Members);

            if (model.Presences != null)
                Update(model.Presences);
        }

        internal void Update(GuildSyncModel model)
        {
            Update(model.Members);

            if (_members.Count != model.Members.Length)
            {
                foreach (var key in _members.Keys.Except(model.Members.Select(x => new Snowflake(x.User.Id))))
                    _members.TryRemove(key, out _);
            }

            Update(model.Presences);
        }

        internal void Update(RoleModel[] models)
        {
            for (var i = 0; i < models.Length; i++)
            {
                var roleModel = models[i];
                _roles.AddOrUpdate(roleModel.Id, _ => new CachedRole(this, roleModel), (_, old) =>
                {
                    old.Update(roleModel);
                    return old;
                });
            }

            if (models.Length != _roles.Count)
            {
                foreach (var key in _roles.Keys)
                {
                    var found = false;
                    for (var i = 0; i < models.Length; i++)
                    {
                        if (key == models[i].Id)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                        _roles.TryRemove(key, out _);
                }
            }
        }

        internal void Update(EmojiModel[] models)
        {
            for (var i = 0; i < models.Length; i++)
            {
                var emojiModel = models[i];
                _emojis.AddOrUpdate(emojiModel.Id.Value, _ => new CachedGuildEmoji(this, emojiModel), (_, old) =>
                {
                    old.Update(emojiModel);
                    return old;
                });
            }

            if (models.Length != _emojis.Count)
            {
                foreach (var key in _emojis.Keys)
                {
                    var found = false;
                    for (var i = 0; i < models.Length; i++)
                    {
                        if (key == models[i].Id)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                        _emojis.TryRemove(key, out _);
                }
            }
        }

        internal void Update(MemberModel[] models)
        {
            for (var i = 0; i < models.Length; i++)
            {
                var memberModel = models[i];
                Client.State.AddOrUpdateMember(this, memberModel, memberModel.User, true);
            }
        }

        internal void Update(PresenceUpdateModel[] models)
        {
            for (var i = 0; i < models.Length; i++)
            {
                var presenceModel = models[i];
                if (_members.TryGetValue(presenceModel.User.Id, out var member))
                    member.Update(presenceModel);
            }
        }

        internal void Update(WebSocketGuildModel model)
        {
            Update(model as GuildModel);

            for (var i = 0; i < model.Channels.Length; i++)
            {
                var channelModel = model.Channels[i];
                _channels.AddOrUpdate(channelModel.Id,
                    _ => CachedGuildChannel.Create(this, channelModel),
                    (_, old) =>
                    {
                        old.Update(channelModel);
                        return old;
                    });
            }

            Update(model.Members);

            for (var i = 0; i < model.VoiceStates.Length; i++)
            {
                var voiceState = model.VoiceStates[i];
                _members[voiceState.UserId].Update(voiceState);
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

            if (model.DiscoverySplash.HasValue)
                DiscoverySplashHash = model.DiscoverySplash.Value;

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
                Update(model.Roles.Value);

            if (model.Emojis.HasValue)
                Update(model.Emojis.Value);

            if (model.Features.HasValue)
                Features = model.Features.Value.ReadOnly();

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
            {
                MemberCount--;
                member.SharedUser.References--;
            }
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
            => Discord.Cdn.GetGuildIconUrl(Id, IconHash, format, size);

        public string GetSplashUrl(int size = 2048)
            => Discord.Cdn.GetGuildSplashUrl(Id, SplashHash, ImageFormat.Png, size);

        public string GetDiscoverySplashUrl(int size = 2048)
            => Discord.Cdn.GetGuildDiscoverySplashUrl(Id, DiscoverySplashHash, ImageFormat.Png, size);

        public string GetBannerUrl(int size = 2048)
            => Discord.Cdn.GetGuildBannerUrl(Id, BannerHash, ImageFormat.Png, size);

        public override string ToString()
            => Name;
    }
}