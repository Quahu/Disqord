using System.Collections.Generic;
using Disqord.Models;
using Qommon.Collections;

namespace Disqord
{
    /// <inheritdoc cref="IApplicationDefaultAuthorizationParameters"/>
    public class TransientApplicationDefaultAuthorizationParameters : TransientEntity<InstallParamsJsonModel>, IApplicationDefaultAuthorizationParameters
    {
        /// <inheritdoc/>
        public IReadOnlyList<string> Scopes => Model.Scopes.ToReadOnlyList();

        /// <inheritdoc/>
        public GuildPermissions Permissions => Model.Permissions;

        public TransientApplicationDefaultAuthorizationParameters(InstallParamsJsonModel model)
            : base(model)
        { }
    }
}
