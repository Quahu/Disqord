namespace Disqord
{
    public static partial class CdnEntityExtensions
    {
        public static string GetAvatarUrl(this IUser user, ImageFormat format = default, int size = 2048) 
            => user.AvatarHash != null
                ? Discord.Cdn.GetUserAvatarUrl(user.Id, user.AvatarHash, format, size)
                : Discord.Cdn.GetDefaultUserAvatarUrl(user.Discriminator);
    }
}
