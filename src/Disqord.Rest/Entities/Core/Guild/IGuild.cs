using System.Collections.Generic;
using System.Globalization;

namespace Disqord
{
    public partial interface IGuild : ISnowflakeEntity, IDeletable
    {
        string Name { get; }

        string IconHash { get; }

        string SplashHash { get; }

        string DiscoverySplashHash { get; }

        Snowflake OwnerId { get; }

        string VoiceRegionId { get; }

        Snowflake? AfkChannelId { get; }

        int AfkTimeout { get; }

        Snowflake? EmbedChannelId { get; }

        bool IsEmbedEnabled { get; }

        VerificationLevel VerificationLevel { get; }

        DefaultNotificationLevel DefaultNotificationLevel { get; }

        ContentFilterLevel ContentFilterLevel { get; }

        IReadOnlyDictionary<Snowflake, IRole> Roles { get; }

        IReadOnlyDictionary<Snowflake, IGuildEmoji> Emojis { get; }

        IReadOnlyList<string> Features { get; }

        MfaLevel MfaLevel { get; }

        Snowflake? ApplicationId { get; }

        bool IsWidgetEnabled { get; }

        Snowflake? WidgetChannelId { get; }

        Snowflake? SystemChannelId { get; }

        int MaxPresenceCount { get; }

        int MaxMemberCount { get; }

        string VanityUrlCode { get; }

        string Description { get; }

        string BannerHash { get; }

        BoostTier BoostTier { get; }

        int BoostingMemberCount { get; }

        CultureInfo PreferredLocale { get; }

        string GetIconUrl(ImageFormat format = default, int size = 2048);

        string GetSplashUrl(int size = 2048);

        string GetDiscoverySplashUrl(int size = 2048);

        string GetBannerUrl(int size = 2048);
    }
}
