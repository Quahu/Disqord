using System.Globalization;

namespace Disqord;

/// <summary>
///     Represents the current user.
/// </summary>
public interface ICurrentUser : IUser
{
    /// <summary>
    ///     Gets the locale of this user.
    /// </summary>
    CultureInfo Locale { get; }

    /// <summary>
    ///     Gets whether this user is verified.
    /// </summary>
    bool IsVerified { get; }

    /// <summary>
    ///     Gets the email of this user.
    /// </summary>
    string? Email { get; }

    /// <summary>
    ///     Gets whether this user has MFA enabled.
    /// </summary>
    bool HasMfaEnabled { get; }

    /// <summary>
    ///     Gets the <see cref="UserFlags"/> of this user.
    /// </summary>
    UserFlags Flags { get; }

    /// <summary>
    ///     Gets the optional <see cref="Disqord.NitroType"/> of this user.
    /// </summary>
    NitroType? NitroType { get; }
}
