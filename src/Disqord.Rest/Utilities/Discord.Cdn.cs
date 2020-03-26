using System;

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
            public const string URL = "https://cdn.discordapp.com/";

            /// <summary>
            ///     Returns the url for a custom emoji.
            /// </summary>
            /// <param name="emojiId"> The custom emoji's id. </param>
            /// <param name="isAnimated"> Whether the custom emoji is animated or not. </param>
            /// <param name="size"> The size of the custom emoji. </param>
            /// <returns>
            ///     The url of the custom emoji.
            /// </returns>
            public static string GetCustomEmojiUrl(Snowflake emojiId, bool isAnimated, int size = 2048)
                => FormatImageUrl($"emojis/{emojiId}", isAnimated ? ImageFormat.Gif : ImageFormat.Png, size);

            /// <summary>
            ///     Returns the url for a guild's icon.
            /// </summary>
            /// <param name="guildId"> The guild's id. </param>
            /// <param name="iconHash"> The guild's icon hash. </param>
            /// <param name="format"> The format to use. </param>
            /// <param name="size"> The size of the guild icon. </param>
            /// <returns>
            ///     The url of the guild icon.
            /// </returns>
            public static string GetGuildIconUrl(Snowflake guildId, string iconHash, ImageFormat format = default, int size = 2048)
                => FormatImageUrl($"icons/{guildId}/{iconHash}", format != default ? format : ImageFormat.Png, size);

            /// <summary>
            ///     Returns the url for a guild's splash.
            /// </summary>
            /// <param name="guildId"> The guild's id. </param>
            /// <param name="splashHash"> The guild's splash hash. </param>
            /// <param name="format"> The format to use. </param>
            /// <param name="size"> The size of the guild splash. </param>
            /// <returns>
            ///     The url of the guild's splash.
            /// </returns>
            public static string GetGuildSplashUrl(Snowflake guildId, string splashHash, ImageFormat format = default, int size = 2048)
                => FormatImageUrl($"splashes/{guildId}/{splashHash}", format != default ? format : ImageFormat.Png, size);

            /// <summary>
            ///     Returns the url for a guild's discovery splash.
            /// </summary>
            /// <param name="guildId"> The guild's id. </param>
            /// <param name="discoverySplashHash"> The guild's discovery splash hash. </param>
            /// <param name="format"> The format to use. </param>
            /// <param name="size"> The size of the discovery guild splash. </param>
            /// <returns>
            ///     The url of the guild's discovery splash.
            /// </returns>
            public static string GetGuildDiscoverySplashUrl(Snowflake guildId, string discoverySplashHash, ImageFormat format = default, int size = 2048)
                => FormatImageUrl($"discovery-splashes/{guildId}/{discoverySplashHash}", format != default ? format : ImageFormat.Png, size);

            /// <summary>
            ///     Returns the url for a guild's banner.
            /// </summary>
            /// <param name="guildId"> The guild's id. </param>
            /// <param name="bannerHash"> The guild's banner hash. </param>
            /// <param name="format"> The format to use. </param>
            /// <param name="size"> The size of the guild banner. </param>
            /// <returns>
            ///     The url of the guild's banner.
            /// </returns>
            public static string GetGuildBannerUrl(Snowflake guildId, string bannerHash, ImageFormat format = default, int size = 2048)
                => FormatImageUrl($"banners/{guildId}/{bannerHash}", format != default ? format : ImageFormat.Png, size);

            /// <summary>
            ///     Returns the url for a default user avatar.
            /// </summary>
            /// <param name="userDiscriminator"> The user's discriminator. </param>
            /// <returns>
            ///     The url of the default user avatar.
            /// </returns>
            public static string GetDefaultUserAvatarUrl(string userDiscriminator)
                => FormatImageUrl($"embed/avatars/{ushort.Parse(userDiscriminator) % 5}", ImageFormat.Png);

            /// <summary>
            ///     Returns the url for a default user avatar.
            /// </summary>
            /// <param name="color"> The avatar color to return. </param>
            /// <returns>
            ///     The url of the default user avatar.
            /// </returns>
            public static string GetDefaultUserAvatarUrl(DefaultAvatarColor color)
                => FormatImageUrl($"embed/avatars/{(byte) color}", ImageFormat.Png);

            /// <summary>
            ///     Returns the url for a default user avatar.
            /// </summary>
            /// <param name="color"> The avatar color to return. </param>
            /// <returns>
            ///     The url of the default user avatar.
            /// </returns>
            public static string GetDefaultUserAvatarUrl(byte color)
                => FormatImageUrl($"embed/avatars/{color}", ImageFormat.Png);

            /// <summary>
            ///     Returns the url for a user's avatar.
            /// </summary>
            /// <param name="userId"> The user's id. </param>
            /// <param name="avatarHash"> The user's avatar hash. </param>
            /// <param name="format"> The format to use. </param>
            /// <param name="size"> The size of the user's avatar. </param>
            /// <returns>
            ///     The url of the user's avatar.
            /// </returns>
            public static string GetUserAvatarUrl(Snowflake userId, string avatarHash, ImageFormat format = default, int size = 2048)
                => FormatImageUrl($"avatars/{userId}/{avatarHash}", format != default ? format : (avatarHash.StartsWith("a_") ? ImageFormat.Gif : ImageFormat.Png), size);

            /// <summary>
            ///     Returns the url for an application's icon url.
            /// </summary>
            /// <param name="applicationId"> The application's id. </param>
            /// <param name="iconHash"> The application's icon hash. </param>
            /// <param name="format"> The format to use. </param>
            /// <param name="size"> The size of the application's icon. </param>
            /// <returns>
            ///     The url of the application's icon.
            /// </returns>
            public static string GetApplicationIconUrl(Snowflake applicationId, string iconHash, ImageFormat format = default, int size = 2048)
                => FormatImageUrl($"app-icons/{applicationId}/{iconHash}", format != default ? format : ImageFormat.Png, size);

            private static string FormatImageUrl(string path, ImageFormat format, int size = 0)
            {
                if (size > 0 && (size < 16 || size > 2048 || (size & -size) != size))
                    throw new ArgumentOutOfRangeException(nameof(size), "Size must be a power of 2 between 16 and 2048.");

                return $"{URL}{path}.{GetImageFormatString(format)}{(size > 0 ? $"?size={size}" : "")}";
            }

            private static string GetImageFormatString(ImageFormat format) => format switch
            {
                ImageFormat.Png => "png",
                ImageFormat.Jpg => "jpg",
                ImageFormat.WebP => "webp",
                ImageFormat.Gif => "gif",
                _ => throw new ArgumentOutOfRangeException(nameof(format), "The image format must be set."),
            };
        }
    }
}