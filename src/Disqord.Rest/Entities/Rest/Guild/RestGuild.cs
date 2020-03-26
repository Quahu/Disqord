using System.Collections.Generic;
using System.Globalization;
using Disqord.Collections;
using Disqord.Models;

namespace Disqord.Rest
{
    public sealed partial class RestGuild : RestSnowflakeEntity, IGuild
    {
        public string Name { get; private set; }

        public string IconHash { get; private set; }

        public string SplashHash { get; private set; }

        public string DiscoverySplashHash { get; private set; }

        public Snowflake OwnerId { get; private set; }

        public string VoiceRegionId { get; private set; }

        public Snowflake? AfkChannelId { get; private set; }

        public int AfkTimeout { get; private set; }

        public bool IsEmbedEnabled { get; private set; }

        public Snowflake? EmbedChannelId { get; private set; }

        public VerificationLevel VerificationLevel { get; private set; }

        public DefaultNotificationLevel DefaultNotificationLevel { get; private set; }

        public ContentFilterLevel ContentFilterLevel { get; private set; }

        public IReadOnlyDictionary<Snowflake, RestRole> Roles { get; private set; }

        public IReadOnlyDictionary<Snowflake, RestGuildEmoji> Emojis { get; private set; }

        public IReadOnlyList<string> Features { get; private set; }

        public MfaLevel MfaLevel { get; private set; }

        public Snowflake? ApplicationId { get; private set; }

        public bool IsWidgetEnabled { get; private set; }

        public Snowflake? WidgetChannelId { get; private set; }

        public Snowflake? SystemChannelId { get; private set; }

        public int MaxPresenceCount { get; private set; }

        public int MaxMemberCount { get; private set; }

        public string VanityUrlCode { get; private set; }

        public string Description { get; private set; }

        public string BannerHash { get; private set; }

        public BoostTier BoostTier { get; private set; }

        public int BoostingMemberCount { get; private set; }

        public CultureInfo PreferredLocale { get; private set; }

        IReadOnlyDictionary<Snowflake, IRole> IGuild.Roles => new ReadOnlyUpcastingDictionary<Snowflake, RestRole, IRole>(Roles);
        IReadOnlyDictionary<Snowflake, IGuildEmoji> IGuild.Emojis => new ReadOnlyUpcastingDictionary<Snowflake, RestGuildEmoji, IGuildEmoji>(Emojis);

        internal RestGuild(RestDiscordClient client, GuildModel model) : base(client, model.Id)
        {
            Update(model);
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

            if (model.EmbedEnabled.HasValue)
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
                Roles = model.Roles.Value.ToReadOnlyDictionary((x, _) => new Snowflake(x.Id), (x, @this) =>
                {
                    var role = new RestRole(@this.Client, @this.Id, x);
                    role.Guild.Value = @this;
                    return role;
                }, this);

            if (model.Emojis.HasValue)
                Emojis = model.Emojis.Value.ToReadOnlyDictionary((x, _) => new Snowflake(x.Id.Value), (x, @this) =>
                {
                    var emoji = new RestGuildEmoji(@this.Client, @this.Id, x);
                    emoji.Guild.Value = @this;
                    return emoji;
                }, this);

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