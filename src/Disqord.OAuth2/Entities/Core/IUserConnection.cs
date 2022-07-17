using System.Collections.Generic;

namespace Disqord.OAuth2;

/// <summary>
///     Represents a connection object that a user has attached to their account.
///     E.g. a <c>GitHub</c> account.
/// </summary>
public interface IUserConnection : INamableEntity
{
    /// <summary>
    ///     Gets the ID of this connection.
    /// </summary>
    string Id { get; }

    /// <summary>
    ///     Gets the type of this connection.
    ///     E.g. <c>twitch</c>, <c>youtube</c>.
    /// </summary>
    string Type { get; }

    /// <summary>
    ///     Gets whether this connection has been revoked.
    /// </summary>
    bool IsRevoked { get; }

    /// <summary>
    ///     Gets the integrations tied to this connection.
    /// </summary>
    IReadOnlyList<IIntegration> Integrations { get; }

    /// <summary>
    ///     Gets whether this connection is verified.
    /// </summary>
    bool IsVerified { get; }

    /// <summary>
    ///     Gets whether friend sync is enabled for this connection.
    /// </summary>
    bool HasFriendSync { get; }

    /// <summary>
    ///     Gets whether activities related to this connection are shown in the user's presence.
    /// </summary>
    bool ShowsActivity { get; }

    /// <summary>
    ///     Gets the visibility of this connection.
    /// </summary>
    UserConnectionVisibility Visibility { get; }
}