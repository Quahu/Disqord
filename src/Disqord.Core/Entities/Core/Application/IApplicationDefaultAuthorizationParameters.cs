using System.Collections.Generic;

namespace Disqord;

/// <summary>
///     Represents the default authorization parameters of an application.
/// </summary>
public interface IApplicationDefaultAuthorizationParameters : IEntity
{
    /// <summary>
    ///     Gets the OAuth2 scopes required by the application.
    /// </summary>
    IReadOnlyList<string> Scopes { get; }

    /// <summary>
    ///     Gets the permissions requested by the application.
    /// </summary>
    Permissions Permissions { get; }
}
