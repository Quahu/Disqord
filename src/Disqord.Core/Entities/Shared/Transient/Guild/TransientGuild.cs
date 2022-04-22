using System;
using System.Collections.Generic;
using System.Globalization;
using Disqord.Models;
using Qommon;
using Qommon.Collections;
using Qommon.Collections.ReadOnly;

namespace Disqord
{
    // If you update any members of this class, make sure to do the same for the gateway equivalent.

    public class TransientGuild : TransientClientEntity<GuildJsonModel>, IGuild
    {
        public Snowflake Id => Model.Id;

        public string Name => Model.Name;

        public string IconHash => Model.Icon;

        public string SplashHash => Model.Splash;

        public string DiscoverySplashHash => Model.DiscoverySplash.GetValueOrDefault();

        public Snowflake OwnerId => Model.OwnerId;

        public Snowflake? AfkChannelId => Model.AfkChannelId;

        public TimeSpan AfkTimeout => TimeSpan.FromSeconds(Model.AfkTimeout);

        public bool IsWidgetEnabled => Model.WidgetEnabled.GetValueOrDefault();

        public Snowflake? WidgetChannelId => Model.WidgetChannelId.GetValueOrDefault();

        public GuildVerificationLevel VerificationLevel => Model.VerificationLevel;

        public GuildNotificationLevel NotificationLevel => Model.DefaultMessageNotifications;

        public GuildContentFilterLevel ContentFilterLevel => Model.ExplicitContentFilter;

        public IReadOnlyDictionary<Snowflake, IRole> Roles => _roles ??= Model.Roles.ToReadOnlyDictionary((Client, Id),
            (model, _) => model.Id,
            (model, state) =>
            {
                var (client, guildId) = state;
                return new TransientRole(client, guildId, model) as IRole;
            });
        private IReadOnlyDictionary<Snowflake, IRole> _roles;

        public IReadOnlyDictionary<Snowflake, IGuildEmoji> Emojis => _emojis ??= Model.Emojis.ToReadOnlyDictionary((Client, Id),
            (model, _) => model.Id.Value,
            (model, state) =>
            {
                var (client, guildId) = state;
                return new TransientGuildEmoji(client, guildId, model) as IGuildEmoji;
            });
        private IReadOnlyDictionary<Snowflake, IGuildEmoji> _emojis;

        public IReadOnlyList<string> Features => Model.Features;

        public GuildMfaLevel MfaLevel => Model.MfaLevel;

        public Snowflake? ApplicationId => Model.ApplicationId;

        public Snowflake? SystemChannelId => Model.SystemChannelId;

        public SystemChannelFlag SystemChannelFlags => Model.SystemChannelFlags;

        public Snowflake? RulesChannelId => Model.RulesChannelId;

        public int? MaxPresenceCount => Model.MaxPresences.GetValueOrDefault();

        public int? MaxMemberCount => Model.MaxMembers.GetValueOrNullable();

        public string VanityUrlCode => Model.VanityUrlCode;

        public string Description => Model.Description;

        public string BannerHash => Model.Banner;

        public GuildBoostTier BoostTier => Model.PremiumTier;

        public int? BoostingMemberCount => Model.PremiumSubscriptionCount.GetValueOrNullable();

        public CultureInfo PreferredLocale => Discord.Internal.GetLocale(Model.PreferredLocale);

        public Snowflake? PublicUpdatesChannelId => Model.PublicUpdatesChannelId;

        public int? MaxVideoMemberCount => Model.MaxVideoChannelUsers.GetValueOrNullable();

        public GuildNsfwLevel NsfwLevel => Model.NsfwLevel;

        public IReadOnlyDictionary<Snowflake, IGuildSticker> Stickers
        {
            get
            {
                if (!Model.Stickers.HasValue)
                    return ReadOnlyDictionary<Snowflake, IGuildSticker>.Empty;

                return _stickers ??= Model.Stickers.Value.ToReadOnlyDictionary(Client,
                    (model, _) => model.Id,
                    (model, client) => new TransientGuildSticker(client, model) as IGuildSticker);
            }
        }
        private IReadOnlyDictionary<Snowflake, IGuildSticker> _stickers;

        public bool IsBoostProgressBarEnabled => Model.PremiumProgressBarEnabled;

        public TransientGuild(IClient client, GuildJsonModel model)
            : base(client, model)
        { }

        public override string ToString()
            => this.GetString();
    }
}
