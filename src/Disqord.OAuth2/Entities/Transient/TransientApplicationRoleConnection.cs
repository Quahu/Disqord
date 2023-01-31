using System.Collections.Generic;
using Disqord.Models;

namespace Disqord.OAuth2;

public class TransientApplicationRoleConnection : TransientEntity<ApplicationRoleConnectionJsonModel>, IApplicationRoleConnection
{
    /// <inheritdoc/>
    public string? PlatformName => Model.PlatformName;

    /// <inheritdoc/>
    public string? PlatformUsername => Model.PlatformUsername;

    /// <inheritdoc/>
    public IReadOnlyDictionary<string, string> Metadata => Model.Metadata;

    public TransientApplicationRoleConnection(ApplicationRoleConnectionJsonModel model)
        : base(model)
    { }
}
