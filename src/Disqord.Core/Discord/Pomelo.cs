using System;
using Qommon;

namespace Disqord;

/// <summary>
///     Represents Discord's new name system utilities.
/// </summary>
public static class Pomelo
{
    /// <summary>
    ///     The general Help Center article link.
    /// </summary>
    public const string GeneralArticleLink = "https://dis.gd/usernames";

    /// <summary>
    ///     The developer Help Center article link.
    /// </summary>
    public const string DeveloperArticleLink = "https://dis.gd/app-usernames";

    /// <summary>
    ///     Represents the obsoletion warning for discriminators.
    /// </summary>
    public const string DiscriminatorObsoletion = "Discriminators are being replaced by the new unique name system (<c>@name</c>). "
        + $"See {DeveloperArticleLink} for more information.";

    /// <summary>
    ///     Checks whether the user has migrated their name from the old
    ///     <c>name#discriminator</c> format to the new <c>@name</c> format.
    /// </summary>
    /// <param name="user"> The user to check. </param>
    /// <returns>
    ///     <see langword="true"/> if the user has migrated their name.
    /// </returns>
    public static bool HasMigratedName(this IUser user)
    {
        Guard.IsNotNull(user);

#pragma warning disable CS0618
        var discriminator = user.Discriminator.AsSpan();
#pragma warning restore CS0618
        foreach (var character in discriminator)
        {
            if (character != '0')
                return false;
        }

        return true;
    }
}
