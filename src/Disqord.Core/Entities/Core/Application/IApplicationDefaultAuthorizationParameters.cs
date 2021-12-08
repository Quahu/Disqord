using System.Collections.Generic;

namespace Disqord
{
    /// <summary>
    ///     Gets the default authorization parameters of the application.
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
        GuildPermissions Permissions { get; }
    }
}
