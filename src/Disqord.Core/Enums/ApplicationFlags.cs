using System;

namespace Disqord;

/// <summary>
///     Represents flags of a Discord application.
/// </summary>
[Flags]
public enum ApplicationFlags
{
    /// <summary>
    ///     The application can connect to the gateway with the presences intent.
    /// </summary>
    Presences = 1 << 12,

    /// <summary>
    ///     The application cannot connect to the gateway with the presences intent.
    /// </summary>
    PresencesLimited = 1 << 13,

    /// <summary>
    ///     The application can connect to the gateway with the members intent.
    /// </summary>
    Members = 1 << 14,

    /// <summary>
    ///     The application cannot connect to the gateway with the members intent.
    /// </summary>
    MembersLimited = 1 << 15,

    /// <summary>
    ///     The application has reached the guild limit and is pending verification.
    /// </summary>
    GuildLimitVerificationPending = 1 << 16,

    /// <summary>
    ///     The application is embedded.
    /// </summary>
    Embedded = 1 << 17,

    /// <summary>
    ///     The application can receive messages over the gateway including their content.
    /// </summary>
    MessageContent = 1 << 18,

    /// <summary>
    ///     The application can receive messages over the gateway excluding their content.
    /// </summary>
    MessageContentLimited = 1 << 19,

    /// <summary>
    ///     The application has registered global application commands.
    /// </summary>
    ApplicationCommandBadge = 1 << 23
}
