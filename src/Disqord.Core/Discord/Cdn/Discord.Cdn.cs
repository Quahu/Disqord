using System.Text;
using Qommon;

namespace Disqord
{
    public static partial class Discord
    {
        /// <summary>
        ///     Provides various Discord CDN utilities.
        /// </summary>
        public static class Cdn
        {
            /// <summary>
            ///     Represents the default Discord CDN url.
            /// </summary>
            public const string BaseAddress = "https://cdn.discordapp.com/";

            public static string GetCustomEmojiUrl(Snowflake emojiId, CdnAssetFormat format = default, int? size = null)
            {
                var path = $"emojis/{emojiId}";
                return FormatUrl(path, format, size);
            }

            public static string GetGuildIconUrl(Snowflake guildId, string iconHash, CdnAssetFormat format = default, int? size = null)
            {
                var path = $"icons/{guildId}/{iconHash}";
                return FormatUrl(path, AutomaticGifFormat(format, iconHash), size);
            }

            public static string GetGuildSplashUrl(Snowflake guildId, string splashHash, CdnAssetFormat format = default, int? size = null)
            {
                var path = $"splashes/{guildId}/{splashHash}";
                return FormatUrl(path, format, size);
            }

            public static string GetGuildDiscoverySplashUrl(Snowflake guildId, string discoverySplashHash, CdnAssetFormat format = default, int? size = null)
            {
                var path = $"discovery-splashes/{guildId}/{discoverySplashHash}";
                return FormatUrl(path, format, size);
            }

            public static string GetGuildBannerUrl(Snowflake guildId, string bannerHash, CdnAssetFormat format = default, int? size = null)
            {
                var path = $"banners/{guildId}/{bannerHash}";
                return FormatUrl(path, AutomaticGifFormat(format, bannerHash), size);
            }

            public static string GetUserBannerUrl(Snowflake userId, string bannerHash, CdnAssetFormat format = default, int? size = null)
            {
                var path = $"banners/{userId}/{bannerHash}";
                return FormatUrl(path, AutomaticGifFormat(format, bannerHash), size);
            }

            public static string GetDefaultAvatarUrl(string discriminator)
                => GetDefaultAvatarUrl((DefaultAvatarColor) (ushort.Parse(discriminator) % 5));

            public static string GetDefaultAvatarUrl(DefaultAvatarColor color)
            {
                var path = $"embed/avatars/{(byte) color}";
                return FormatUrl(path, CdnAssetFormat.Png, null);
            }

            public static string GetAvatarUrl(Snowflake userId, string avatarHash, CdnAssetFormat format = default, int? size = null)
            {
                var path = $"avatars/{userId}/{avatarHash}";
                return FormatUrl(path, AutomaticGifFormat(format, avatarHash), size);
            }

            public static string GetGuildAvatarUrl(Snowflake guildId, Snowflake memberId, string avatarHash, CdnAssetFormat format = default, int? size = null)
            {
                var path = $"guilds/{guildId}/users/{memberId}/avatars/{avatarHash}";
                return FormatUrl(path, AutomaticGifFormat(format, avatarHash), size);
            }

            public static string GetApplicationIconUrl(Snowflake applicationId, string iconHash, CdnAssetFormat format = default, int? size = null)
            {
                var path = $"app-icons/{applicationId}/{iconHash}";
                return FormatUrl(path, format, size);
            }

            public static string GetApplicationCoverUrl(Snowflake applicationId, string coverHash, CdnAssetFormat format = default, int? size = null)
            {
                var path = $"app-icons/{applicationId}/{coverHash}";
                return FormatUrl(path, format, size);
            }

            public static string GetApplicationAssetUrl(Snowflake applicationId, string assetId, CdnAssetFormat format = default, int? size = null)
            {
                var path = $"app-assets/{applicationId}/{assetId}";
                return FormatUrl(path, format, size);
            }

            public static string GetStickerPackBannerUrl(Snowflake bannerAssetId, CdnAssetFormat format = default, int? size = null)
            {
                var path = $"app-assets/710982414301790216/store/{bannerAssetId}";
                return FormatUrl(path, format, size);
            }

            public static string GetTeamIconUrl(Snowflake teamId, string iconHash, CdnAssetFormat format = default, int? size = null)
            {
                var path = $"team-icons/{teamId}/{iconHash}";
                return FormatUrl(path, format, size);
            }

            public static string GetStickerUrl(Snowflake stickerId, StickerFormatType format = StickerFormatType.Png)
            {
                Guard.IsDefined(format);

                var formatString = format switch
                {
                    StickerFormatType.Lottie => "json",
                    _ => "png"
                };

                return $"{BaseAddress}stickers/{stickerId}.{formatString}";
            }

            public static string GetRoleIconUrl(Snowflake roleId, string iconHash, CdnAssetFormat format = default, int? size = null)
            {
                var path = $"role-icons/{roleId}/{iconHash}";
                return FormatUrl(path, format, size);
            }

            public static string GetEventCoverImageUrl(Snowflake eventId, string coverImageHash, CdnAssetFormat format = default, int? size = null)
            {
                var path = $"guild-events/{eventId}/{coverImageHash}";
                return FormatUrl(path, format, size);
            }

            private static string FormatUrl(string path, CdnAssetFormat format, int? size)
            {
                Guard.IsDefined(format);

                if (size != null)
                {
                    var value = size.Value;
                    if (value < 16 || value > 4096 || (value & -value) != value)
                        Throw.ArgumentOutOfRangeException(nameof(size), "Size must be a power of 2 between 16 and 4096.");
                }

                if (format == CdnAssetFormat.Automatic)
                {
                    // If the calling methods didn't overwrite the value we default to PNG.
                    format = CdnAssetFormat.Png;
                }

                var stringBuilder = new StringBuilder(BaseAddress);
                stringBuilder.Append(path);
                var formatString = GetFormatString(format);
                if (formatString != null)
                    stringBuilder.Append('.').Append(formatString);

                if (size != null)
                    stringBuilder.Append("?size=").Append(size);

                return stringBuilder.ToString();
            }

            private static CdnAssetFormat AutomaticGifFormat(CdnAssetFormat format, string hash)
            {
                if (format != CdnAssetFormat.Automatic)
                    return format;

                return hash.StartsWith("a_") ? CdnAssetFormat.Gif : CdnAssetFormat.Png;
            }

            private static string GetFormatString(CdnAssetFormat format)
                => format switch
                {
                    CdnAssetFormat.None => null,
                    CdnAssetFormat.Png => "png",
                    CdnAssetFormat.Jpg => "jpg",
                    CdnAssetFormat.WebP => "webp",
                    CdnAssetFormat.Gif => "gif",
                    _ => Throw.ArgumentOutOfRangeException<string>(nameof(format), "Unknown CDN asset format."),
                };
        }
    }
}
