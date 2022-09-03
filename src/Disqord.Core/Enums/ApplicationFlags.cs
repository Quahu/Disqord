using System;

namespace Disqord;

/// <summary>
///     Represents flags of a Discord application.
/// </summary>
[Flags]
public enum ApplicationFlags : ulong
{
    /// <summary>
    ///     The bot application is verified and can receive presence information.
    /// </summary>
    Presences = 1 << 12,

    /// <summary>
    ///     The bot application is under the verification guild threshold
    ///     and can receive presence information.
    /// </summary>
    PresencesLimited = 1 << 13,

    /// <summary>
    ///     The bot application is verified to receive member information.
    /// </summary>
    Members = 1 << 14,

    /// <summary>
    ///     The bot application is under the verification guild threshold
    ///     and can receive member information.
    /// </summary>
    MembersLimited = 1 << 15,

    /// <summary>
    ///     The bot application has reached the verification guild threshold and is pending verification.
    /// </summary>
    GuildLimitVerificationPending = 1 << 16,

    /// <summary>
    ///     The application is embedded.
    /// </summary>
    Embedded = 1 << 17,

    /// <summary>
    ///     The bot application is verified to receive message content information.
    /// </summary>
    MessageContent = 1 << 18,

    /// <summary>
    ///     The bot application is under the verification guild threshold
    ///     and can receive message content information.
    /// </summary>
    MessageContentLimited = 1 << 19,

    /// <summary>
    ///     The application has registered global application commands.
    /// </summary>
    ApplicationCommandBadge = 1 << 23
}
