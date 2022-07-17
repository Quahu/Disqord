using System.ComponentModel;
using Qommon;

namespace Disqord;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class CdnEntityExtensions
{
    public static string GetUrl(this ICustomEmoji emoji, CdnAssetFormat format = default, int? size = null)
    {
        Guard.IsNotNull(emoji);

        if (format == CdnAssetFormat.Automatic)
        {
            format = emoji.IsAnimated
                ? CdnAssetFormat.Gif
                : CdnAssetFormat.Png;
        }

        return Discord.Cdn.GetCustomEmojiUrl(emoji.Id, format, size);
    }

    public static string? GetIconUrl(this IGuild guild, CdnAssetFormat format = default, int? size = null)
    {
        Guard.IsNotNull(guild);

        var iconHash = guild.IconHash;
        return iconHash != null
            ? Discord.Cdn.GetGuildIconUrl(guild.Id, iconHash, format, size)
            : null;
    }

    public static string? GetSplashUrl(this IGuild guild, CdnAssetFormat format = default, int? size = null)
    {
        Guard.IsNotNull(guild);

        var splashHash = guild.SplashHash;
        return splashHash != null
            ? Discord.Cdn.GetGuildSplashUrl(guild.Id, splashHash, format, size)
            : null;
    }

    public static string? GetDiscoverySplashUrl(this IGuild guild, CdnAssetFormat format = default, int? size = null)
    {
        Guard.IsNotNull(guild);

        var discoverySplashHash = guild.DiscoverySplashHash;
        return discoverySplashHash != null
            ? Discord.Cdn.GetGuildDiscoverySplashUrl(guild.Id, discoverySplashHash, format, size)
            : null;
    }

    public static string? GetBannerUrl(this IGuild guild, CdnAssetFormat format = default, int? size = null)
    {
        Guard.IsNotNull(guild);

        var bannerHash = guild.BannerHash;
        return bannerHash != null
            ? Discord.Cdn.GetGuildBannerUrl(guild.Id, bannerHash, format, size)
            : null;
    }

    public static string GetAvatarUrl(this IUser user, CdnAssetFormat format = default, int? size = null)
    {
        Guard.IsNotNull(user);

        var avatarHash = user.AvatarHash;
        return avatarHash != null
            ? Discord.Cdn.GetAvatarUrl(user.Id, avatarHash, format, size)
            : Discord.Cdn.GetDefaultAvatarUrl(user.Discriminator);
    }

    public static string GetDefaultAvatarUrl(this IUser user)
    {
        Guard.IsNotNull(user);

        return Discord.Cdn.GetDefaultAvatarUrl(user.Discriminator);
    }

    public static string GetGuildAvatarUrl(this IMember member, CdnAssetFormat format = default, int? size = null)
    {
        Guard.IsNotNull(member);

        var avatarHash = member.GuildAvatarHash;
        return avatarHash != null
            ? Discord.Cdn.GetGuildAvatarUrl(member.GuildId, member.Id, avatarHash, format, size)
            : member.GetAvatarUrl(format, size);
    }

    public static string? GetIconUrl(this IApplication application, CdnAssetFormat format = default, int? size = null)
    {
        Guard.IsNotNull(application);

        var iconHash = application.IconHash;
        return iconHash != null
            ? Discord.Cdn.GetApplicationIconUrl(application.Id, iconHash, format, size)
            : null;
    }

    public static string? GetCoverUrl(this IStoreApplication application, CdnAssetFormat format = default, int? size = null)
    {
        Guard.IsNotNull(application);

        var coverHash = application.CoverHash;
        return coverHash != null
            ? Discord.Cdn.GetApplicationCoverUrl(application.Id, coverHash, format, size)
            : null;
    }

    public static string? GetBannerUrl(this IStickerPack stickerPack, CdnAssetFormat format = default, int? size = null)
    {
        Guard.IsNotNull(stickerPack);

        var bannerAssetId = stickerPack.BannerAssetId;
        return bannerAssetId != null
            ? Discord.Cdn.GetStickerPackBannerUrl(bannerAssetId.Value, format, size)
            : null;
    }

    public static string? GetIconUrl(this IApplicationTeam team, CdnAssetFormat format = default, int? size = null)
    {
        Guard.IsNotNull(team);

        var iconHash = team.IconHash;
        return iconHash != null
            ? Discord.Cdn.GetTeamIconUrl(team.Id, iconHash, format, size)
            : null;
    }

    public static string? GetIconUrl(this IRole role, CdnAssetFormat format = default, int? size = null)
    {
        Guard.IsNotNull(role);

        var iconHash = role.IconHash;
        return iconHash != null
            ? Discord.Cdn.GetRoleIconUrl(role.Id, iconHash, format, size)
            : null;
    }

    public static string? GetCoverImageUrl(this IGuildEvent @event, CdnAssetFormat format = default, int? size = null)
    {
        Guard.IsNotNull(@event);

        var coverImageHash = @event.CoverImageHash;
        return coverImageHash != null
            ? Discord.Cdn.GetEventCoverImageUrl(@event.Id, coverImageHash, format, size)
            : null;
    }

    public static string GetUrl(this IPartialSticker sticker)
    {
        Guard.IsNotNull(sticker);

        return Discord.Cdn.GetStickerUrl(sticker.Id, sticker.FormatType);
    }
}