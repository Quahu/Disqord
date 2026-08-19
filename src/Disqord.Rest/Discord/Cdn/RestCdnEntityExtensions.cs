namespace Disqord.Rest;

public static class RestCdnEntityExtensions
{
    public static string? GetBannerUrl(this IRestUser user, CdnAssetFormat format = default, int? size = null)
    {
        var bannerHash = user.BannerHash;
        return bannerHash != null
            ? Discord.Cdn.GetUserBannerUrl(user.Id, bannerHash, format, size)
            : null;
    }

    public static string? GetGuildBannerUrl(this IRestMember member, CdnAssetFormat format = default, int? size = null)
    {
        var guildBannerHash = member.GuildBannerHash;
        return guildBannerHash != null
            ? Discord.Cdn.GetGuildMemberBannerUrl(member.GuildId, member.Id, guildBannerHash, format, size)
            : null;
    }
}
