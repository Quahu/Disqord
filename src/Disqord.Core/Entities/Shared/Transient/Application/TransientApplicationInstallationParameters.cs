using System.Collections.Generic;
using Disqord.Models;
using Qommon.Collections;

namespace Disqord
{
    public class TransientApplicationInstallationParameters : TransientEntity<InstallParamsJsonModel>, IApplicationInstallationParameters
    {
        public IReadOnlyList<string> Scopes => Model.Scopes.ToReadOnlyList();

        public GuildPermissions Permissions => Model.Permissions;

        public TransientApplicationInstallationParameters(InstallParamsJsonModel model)
            : base(model)
        { }
    }
}
