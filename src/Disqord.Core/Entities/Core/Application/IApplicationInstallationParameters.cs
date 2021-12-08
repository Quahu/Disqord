using System.Collections.Generic;

namespace Disqord
{
    public interface IApplicationInstallationParameters : IEntity
    {
        /// <summary>
        ///     Gets the scopes required by the application.
        /// </summary>
        IReadOnlyList<string> Scopes { get; }

        /// <summary>
        ///     Gets the permissions requested by the application.
        /// </summary>
        GuildPermissions Permissions { get; }
    }
}
