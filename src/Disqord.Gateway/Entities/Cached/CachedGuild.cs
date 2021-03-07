using System;
using System.Collections.Generic;
using System.Globalization;
using Disqord.Collections;
using Disqord.Gateway.Api.Models;
using Disqord.Models;

namespace Disqord.Gateway
{
    public class CachedGuild : CachedSnowflakeEntity, IGatewayGuild, IJsonUpdatable<UnavailableGuildJsonModel>
    {
        // Interface: INamable
        public string Name { get; private set; }

        // Interface: IGuild
        public string IconHash { get; private set; }

        public string SplashHash { get; private set; }

        public string DiscoverySplashHash { get; private set; }

        public Snowflake OwnerId { get; private set; }

        public string VoiceRegionId { get; private set; }

        public Snowflake? AfkChannelId { get; private set; }

        public int AfkTimeout { get; private set; }

        public bool IsWidgetEnabled { get; private set; }

        public Snowflake? WidgetChannelId { get; private set; }

        public GuildVerificationLevel VerificationLevel { get; private set; }

        public GuildNotificationLevel NotificationLevel { get; private set; }

        public GuildContentFilterLevel ContentFilterLevel { get; private set; }

        public IReadOnlyDictionary<Snowflake, IRole> Roles
        {
            get
            {
                if (Client.CacheProvider.TryGetRoles(Id, out var cache))
                    return new ReadOnlyUpcastingDictionary<Snowflake, CachedRole, IRole>(cache.ReadOnly());

                return ReadOnlyDictionary<Snowflake, IRole>.Empty;
            }
        }

        public IReadOnlyDictionary<Snowflake, IGuildEmoji> Emojis { get; private set; }

        public IReadOnlyList<string> Features { get; private set; }

        public GuildMfaLevel MfaLevel { get; private set; }

        public Snowflake? ApplicationId { get; }

        public Snowflake? SystemChannelId { get; private set; }

        public GuildSystemChannelFlags SystemChannelFlags { get; private set; }

        public Snowflake? RulesChannelId { get; private set; }

        public int MaxPresenceCount { get; private set; }

        public int MaxMemberCount { get; private set; }

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

        public int MemberCount { get; private set; }

        public IReadOnlyDictionary<Snowflake, IMember> Members { get; }

        public IReadOnlyDictionary<Snowflake, IGuildChannel> Channels { get; }

        public CachedGuild(IGatewayClient client, GatewayGuildJsonModel model)
            : base(client, model.Id)
        {
            ApplicationId = model.ApplicationId;

            Update(model);
        }

        public void Update(GuildJsonModel model)
        {
            Name = model.Name;
        }

        public void Update(GatewayGuildJsonModel model)
        {
            JoinedAt = model.JoinedAt;
            IsLarge = model.Large;

            if (model.Unavailable.HasValue)
                IsUnavailable = model.Unavailable.Value;

            Update(model as GuildJsonModel);
        }

        public void Update(UnavailableGuildJsonModel model)
        {
            if (model.Unavailable.HasValue)
                IsUnavailable = model.Unavailable.Value;
        }
    }
}
