using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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

        public string? DiscoverySplashHash => Model.DiscoverySplash.GetValueOrDefault();

        public Snowflake OwnerId => Model.OwnerId;

        public string VoiceRegionId => Model.Region;

        public Snowflake? AfkChannelId => Model.AfkChannelId;

        public int AfkTimeout => Model.AfkTimeout;

        public bool IsWidgetEnabled => Model.WidgetEnabled.GetValueOrDefault();

        public Snowflake? WidgetChannelId => Model.WidgetChannelId.GetValueOrDefault();

        public GuildVerificationLevel VerificationLevel => Model.VerificationLevel;

        public GuildNotificationLevel NotificationLevel => Model.DefaultMessageNotifications;

        public GuildContentFilterLevel ContentFilterLevel => Model.ExplicitContentFilter;

        public IReadOnlyDictionary<Snowflake, IRole> Roles
        {
            get
            {
                if (_roles == null)
                    _roles = Model.Roles.ToReadOnlyDictionary((Client, Id), (x, _) => x.Id, (x, tuple) =>
                    {
                        var (client, guildId) = tuple;
                        return new TransientRole(client, guildId, x) as IRole;
                    });

                return _roles;
            }
        }
        private IReadOnlyDictionary<Snowflake, IRole>? _roles;

        public IReadOnlyDictionary<Snowflake, IGuildEmoji> Emojis { get; }

        public IReadOnlyList<string> Features { get; }

        public GuildMfaLevel MfaLevel { get; }

        public Snowflake? ApplicationId { get; }

        public Snowflake? SystemChannelId { get; }

        public GuildSystemChannelFlags SystemChannelFlags { get; }

        public Snowflake? RulesChannelId { get; }

        public int MaxPresenceCount { get; }

        public int MaxMemberCount { get; }

        public string VanityUrlCode { get; }

        public string Description { get; }

        public string BannerHash { get; }

        public BoostTier BoostTier { get; }

        public int? BoostingMemberCount { get; }

        public CultureInfo PreferredLocale { get; }

        public Snowflake? PublicUpdatesChannelId { get; }

        public int? MaxVideoMemberCount { get; }

        public TransientGuild(IClient client, GuildJsonModel model) : base(client, model)
        { }
    }
}
