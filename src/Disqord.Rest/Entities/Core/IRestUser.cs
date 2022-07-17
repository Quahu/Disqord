namespace Disqord.Rest;

public interface IRestUser : IUser
{
    /// <summary>
    ///     Gets the banner image hash of this user.
    ///     Returns <see langword="null"/> if the user has no banner set.
    /// </summary>
    string? BannerHash { get; }

    /// <summary>
    ///     Gets the accent color of this user.
    ///     Returns <see langword="null"/> if the user has accent color set.
    /// </summary>
    Color? AccentColor { get; }
}
