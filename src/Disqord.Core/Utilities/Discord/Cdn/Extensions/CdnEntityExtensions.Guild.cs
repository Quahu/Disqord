using System;

namespace Disqord
{
    public static partial class CdnEntityExtensions
    {
        public static string GetIconUrl(this IGuild guild, ImageFormat format = default, int size = 2048)
            => Discord.Cdn.GetGuildIconUrl(guild.Id, guild.IconHash, format, size);

        public static string GetSplashUrl(this IGuild guild, int size = 2048)
            => Discord.Cdn.GetGuildSplashUrl(guild.Id, guild.SplashHash, ImageFormat.Png, size);

        public static string GetDiscoverySplashUrl(this IGuild guild, int size = 2048)
            => Discord.Cdn.GetGuildDiscoverySplashUrl(guild.Id, guild.DiscoverySplashHash, ImageFormat.Png, size);

        public static string GetBannerUrl(this IGuild guild, int size = 2048)
            => Discord.Cdn.GetGuildBannerUrl(guild.Id, guild.BannerHash, ImageFormat.Png, size);
    }
}
