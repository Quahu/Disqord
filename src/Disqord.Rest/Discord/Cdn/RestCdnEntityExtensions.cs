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
}
