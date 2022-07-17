using System.Collections.Generic;
using Disqord.Models;
using Qommon.Collections.ReadOnly;

namespace Disqord;

/// <inheritdoc cref="IApplicationCommandGuildPermissions"/>
public class TransientApplicationCommandGuildPermissions : TransientClientEntity<ApplicationCommandGuildPermissionsJsonModel>, IApplicationCommandGuildPermissions
{
    /// <inheritdoc/>
    public Snowflake Id => Model.Id;

    /// <inheritdoc/>
    public Snowflake GuildId => Model.GuildId;

    /// <inheritdoc/>
    public Snowflake ApplicationId => Model.ApplicationId;

    /// <inheritdoc/>
    public IReadOnlyList<IApplicationCommandPermission> Permissions => _permissions ??=
        Model.Permissions.ToReadOnlyList(model => new TransientApplicationCommandPermission(model));

    private IReadOnlyList<IApplicationCommandPermission>? _permissions;

    public TransientApplicationCommandGuildPermissions(IClient client, ApplicationCommandGuildPermissionsJsonModel model)
        : base(client, model)
    { }
}
