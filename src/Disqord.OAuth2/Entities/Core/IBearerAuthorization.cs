using System;
using System.Collections.Generic;
using Disqord.Models;

namespace Disqord.OAuth2;

/// <summary>
///     Represents current <see cref="BearerToken"/> authorization.
/// </summary>
public interface IBearerAuthorization : IClientEntity, IJsonUpdatable<AuthorizationJsonModel>
{
    /// <summary>
    ///     Gets the partial application of this authorization.
    /// </summary>
    IApplication Application { get; }

    /// <summary>
    ///     Gets the scopes used with this authorization.
    /// </summary>
    IReadOnlyList<string> Scopes { get; }

    /// <summary>
    ///     Gets when the access token of this authorization expires.
    /// </summary>
    DateTimeOffset ExpiresAt { get; }

    /// <summary>
    ///     Gets the user of this authorization.
    ///     Returns <see langword="null"/>, if the scopes do not contain the <c>identify</c> scope.
    /// </summary>
    IUser? User { get; }
}