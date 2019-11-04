using System.Globalization;
using System.Runtime.CompilerServices;
using Disqord.Rest;

namespace Disqord
{
    public static partial class Discord
    {
        internal static class Internal
        {
            internal static string GetAvatarUrl(RestWebhook webhook, ImageFormat format = default, int size = 2048)
                => webhook.AvatarHash != null
                    ? GetUserAvatarUrl(webhook.Id, webhook.AvatarHash, format, size)
                    : GetDefaultUserAvatarUrl(DefaultAvatarColor.Blurple);

            internal static string GetAvatarUrl(IUser user, ImageFormat format = default, int size = 2048)
                => user.AvatarHash != null
                    ? GetUserAvatarUrl(user.Id, user.AvatarHash, format, size)
                    : GetDefaultUserAvatarUrl(user.Discriminator);

            internal static CultureInfo CreateLocale(string locale)
                => CultureInfo.ReadOnly(new CultureInfo(locale));
        }
    }
}