using System;
using System.Collections.Generic;
using System.Globalization;
using Disqord.Collections;
using Disqord.Models;

namespace Disqord
{
    public class TransientGuild : TransientEntity<GuildJsonModel>, IGuild
    {
        public Snowflake Id => Model.Id;

        public DateTimeOffset CreatedAt => Id.CreatedAt;

        public string Name => Model.Name;

        public string IconHash => Model.Icon;

        public string SplashHash => Model.Splash;

        public string DiscoverySplashHash => Model.DiscoverySplash.GetValueOrDefault();

        public Snowflake OwnerId => Model.OwnerId;

        public string VoiceRegion => Model.Region;

        public Snowflake? AfkChannelId => Model.AfkChannelId;

        public TimeSpan AfkTimeout => TimeSpan.FromSeconds(Model.AfkTimeout);

        public bool IsWidgetEnabled => Model.WidgetEnabled.GetValueOrDefault();

        public Snowflake? WidgetChannelId => Model.WidgetChannelId.GetValueOrDefault();

        public GuildVerificationLevel VerificationLevel => Model.VerificationLevel;

        public GuildNotificationLevel NotificationLevel => Model.DefaultMessageNotifications;

        public GuildContentFilterLevel ContentFilterLevel => Model.ExplicitContentFilter;

        public IReadOnlyDictionary<Snowflake, IRole> Roles => _roles ??= Model.Roles.ToReadOnlyDictionary((Client, Id), (x, _) => x.Id, (x, tuple) =>
        {
            var (client, guildId) = tuple;
            return new TransientRole(client, guildId, x) as IRole;
        });
        private IReadOnlyDictionary<Snowflake, IRole> _roles;

        public IReadOnlyDictionary<Snowflake, IGuildEmoji> Emojis => _emojis ??= Model.Emojis.ToReadOnlyDictionary((Client, Id), (x, _) => x.Id.Value, (x, tuple) =>
        {
            var (client, guildId) = tuple;
            return new TransientGuildEmoji(client, guildId, x) as IGuildEmoji;
        });
        private IReadOnlyDictionary<Snowflake, IGuildEmoji> _emojis;

        public IReadOnlyList<string> Features => Model.Features;

        public GuildMfaLevel MfaLevel => Model.MfaLevel;

        public Snowflake? ApplicationId => Model.ApplicationId;

        public Snowflake? SystemChannelId => Model.SystemChannelId;

        public SystemChannelFlag SystemChannelFlags => Model.SystemChannelFlags;

        public Snowflake? RulesChannelId => Model.RulesChannelId;

        public int? MaxPresenceCount => Optional.Convert(Model.MaxPresences, x => x ?? Discord.Constants.DefaultMaxGuildPresenceCount).GetValueOrNullable();

        public int? MaxMemberCount => Model.MaxMembers.GetValueOrNullable();

        public string VanityUrlCode => Model.VanityUrlCode;

        public string Description => Model.Description;

        public string BannerHash => Model.Banner;

        public BoostTier BoostTier => Model.PremiumTier;

        public int? BoostingMemberCount => Model.PremiumSubscriptionCount.GetValueOrNullable();

        public CultureInfo PreferredLocale => Discord.Internal.GetLocale(Model.PreferredLocale);

        public Snowflake? PublicUpdatesChannelId => Model.PublicUpdatesChannelId;

        public int? MaxVideoMemberCount => Model.MaxVideoChannelUsers.GetValueOrNullable();

        public TransientGuild(IClient client, GuildJsonModel model)
            : base(client, model)
        { }
    }
}
