using System.Collections.Generic;
using Disqord.Models;

namespace Disqord;

/// <inheritdoc cref="IApplicationDefaultAuthorizationParameters"/>
public class TransientApplicationDefaultAuthorizationParameters : TransientEntity<InstallParamsJsonModel>, IApplicationDefaultAuthorizationParameters
{
    /// <inheritdoc/>
    public IReadOnlyList<string> Scopes => Model.Scopes;

    /// <inheritdoc/>
    public Permissions Permissions => Model.Permissions;

    public TransientApplicationDefaultAuthorizationParameters(InstallParamsJsonModel model)
        : base(model)
    { }
}
