using System;
using System.Collections.Generic;
using System.Globalization;
using Disqord.Collections;
using Disqord.Gateway.Api.Models;
using Disqord.Models;

namespace Disqord.Gateway
{
    public class CachedGuild : CachedSnowflakeEntity, IGatewayGuild, IJsonUpdatable<UnavailableGuildJsonModel>, IJsonUpdatable<GuildEmojisUpdateJsonModel>
    {
        // Interface: INamable
        public string Name { get; private set; }

        // Interface: IGuild
        public string IconHash { get; private set; }

        public string SplashHash { get; private set; }

        public string DiscoverySplashHash { get; private set; }

        public Snowflake OwnerId { get; private set; }

        public string VoiceRegion { get; private set; }

        public Snowflake? AfkChannelId { get; private set; }

        public TimeSpan AfkTimeout { get; private set; }

        public bool IsWidgetEnabled { get; private set; }

        public Snowflake? WidgetChannelId { get; private set; }

        public GuildVerificationLevel VerificationLevel { get; private set; }

        public GuildNotificationLevel NotificationLevel { get; private set; }

        public GuildContentFilterLevel ContentFilterLevel { get; private set; }

        public IReadOnlyDictionary<Snowflake, IRole> Roles
        {
            get
            {
                if (Client.CacheProvider.TryGetRoles(Id, out var cache, true))
                    return new ReadOnlyUpcastingDictionary<Snowflake, CachedRole, IRole>(cache.ReadOnly());

                return ReadOnlyDictionary<Snowflake, IRole>.Empty;
            }
        }

        public IReadOnlyDictionary<Snowflake, IGuildEmoji> Emojis { get; private set; }

        public IReadOnlyList<string> Features { get; private set; }

        public GuildMfaLevel MfaLevel { get; private set; }

        public Snowflake? ApplicationId { get; }

        public Snowflake? SystemChannelId { get; private set; }

        public SystemChannelFlag SystemChannelFlags { get; private set; }

        public Snowflake? RulesChannelId { get; private set; }

        public int? MaxPresenceCount { get; private set; }

        public int? MaxMemberCount { get; private set; }

        public string VanityUrlCode { get; private set; }

        public string Description { get; private set; }

        public string BannerHash { get; private set; }

        public BoostTier BoostTier { get; private set; }

        public int? BoostingMemberCount { get; private set; }

        public CultureInfo PreferredLocale { get; private set; }

        public Snowflake? PublicUpdatesChannelId { get; private set; }

        public int? MaxVideoMemberCount { get; private set; }

        // Interface: IGatewayGuild
        public DateTimeOffset JoinedAt { get; private set; }

        public bool IsLarge { get; private set; }

        public bool IsUnavailable { get; private set; }

        // TODO: track member count
        public int MemberCount { get; private set; }

        public IReadOnlyDictionary<Snowflake, IMember> Members
        {
            get
            {
                if (Client.CacheProvider.TryGetMembers(Id, out var cache, true))
                    return new ReadOnlyUpcastingDictionary<Snowflake, CachedMember, IMember>(cache.ReadOnly());

                return ReadOnlyDictionary<Snowflake, IMember>.Empty;
            }
        }

        IReadOnlyDictionary<Snowflake, IGuildChannel> IGatewayGuild.Channels
        {
            get
            {
                if (Client.CacheProvider.TryGetChannels(Id, out var cache, true))
                    return new ReadOnlyUpcastingDictionary<Snowflake, CachedGuildChannel, IGuildChannel>(cache.ReadOnly());

                return ReadOnlyDictionary<Snowflake, IGuildChannel>.Empty;
            }
        }

        IReadOnlyDictionary<Snowflake, IVoiceState> IGatewayGuild.VoiceStates
        {
            get
            {
                if (Client.CacheProvider.TryGetVoiceStates(Id, out var cache, true))
                    return new ReadOnlyUpcastingDictionary<Snowflake, CachedVoiceState, IVoiceState>(cache.ReadOnly());

                return ReadOnlyDictionary<Snowflake, IVoiceState>.Empty;
            }
        }

        IReadOnlyDictionary<Snowflake, IPresence> IGatewayGuild.Presences
        {
            get
            {
                if (Client.CacheProvider.TryGetPresences(Id, out var cache, true))
                    return new ReadOnlyUpcastingDictionary<Snowflake, CachedPresence, IPresence>(cache.ReadOnly());

                return ReadOnlyDictionary<Snowflake, IPresence>.Empty;
            }
        }

        public CachedGuild(IGatewayClient client, GatewayGuildJsonModel model)
            : base(client, model.Id)
        {
            ApplicationId = model.ApplicationId;

            Update(model);
        }

        public void Update(GuildJsonModel model)
        {
            Name = model.Name;
            IconHash = model.Icon;
            SplashHash = model.Splash;

            if (model.DiscoverySplash.HasValue)
                DiscoverySplashHash = model.DiscoverySplash.Value;

            OwnerId = model.OwnerId;
            VoiceRegion = model.Region;
            AfkChannelId = model.AfkChannelId;
            AfkTimeout = TimeSpan.FromSeconds(model.AfkTimeout);

            if (model.WidgetEnabled.HasValue)
                IsWidgetEnabled = model.WidgetEnabled.Value;

            if (model.WidgetChannelId.HasValue)
                WidgetChannelId = model.WidgetChannelId.Value;

            VerificationLevel = model.VerificationLevel;
            NotificationLevel = model.DefaultMessageNotifications;
            ContentFilterLevel = model.ExplicitContentFilter;
            SetEmojis(model.Emojis);
            Features = model.Features;
            MfaLevel = model.MfaLevel;
            SystemChannelId = model.SystemChannelId;
            SystemChannelFlags = model.SystemChannelFlags;
            RulesChannelId = model.RulesChannelId;
            MaxPresenceCount = model.MaxPresences.GetValueOrDefault();
            MaxMemberCount = model.MaxMembers.GetValueOrNullable();
            VanityUrlCode = model.VanityUrlCode;
            Description = model.Description;
            BannerHash = model.Banner;
            BoostTier = model.PremiumTier;
            BoostingMemberCount = model.PremiumSubscriptionCount.GetValueOrNullable();
            PreferredLocale = Discord.Internal.GetLocale(model.PreferredLocale);
            PublicUpdatesChannelId = model.PublicUpdatesChannelId;
            MaxVideoMemberCount = model.MaxVideoChannelUsers.GetValueOrNullable();
        }

        public void Update(GatewayGuildJsonModel model)
        {
            JoinedAt = model.JoinedAt;
            IsLarge = model.Large;

            if (model.Unavailable.HasValue)
                IsUnavailable = model.Unavailable.Value;

            MemberCount = model.MemberCount;

            Update(model as GuildJsonModel);
        }

        public void Update(UnavailableGuildJsonModel model)
        {
            if (model.Unavailable.HasValue)
                IsUnavailable = model.Unavailable.Value;
        }

        public void Update(GuildEmojisUpdateJsonModel model)
        {
            SetEmojis(model.Emojis);
        }

        private void SetEmojis(EmojiJsonModel[] emojis)
        {
            Emojis = emojis.ToReadOnlyDictionary((Client, Id), (x, _) => x.Id.Value, (x, tuple) =>
            {
                var (client, guildId) = tuple;
                return new TransientGuildEmoji(client, guildId, x) as IGuildEmoji;
            });
        }
    }
}
