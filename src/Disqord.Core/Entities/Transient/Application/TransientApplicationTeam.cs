using System.Collections.Generic;
using Disqord.Models;
using Qommon.Collections.ReadOnly;

namespace Disqord;

/// <inheritdoc cref="IApplicationTeam"/>
public class TransientApplicationTeam : TransientClientEntity<TeamJsonModel>, IApplicationTeam
{
    /// <inheritdoc/>
    public Snowflake Id => Model.Id;

    /// <inheritdoc/>
    public string? IconHash => Model.Icon;

    /// <inheritdoc/>
    public IReadOnlyDictionary<Snowflake, IApplicationTeamMember> Members => _members ??= Model.Members.ToReadOnlyDictionary(Client,
        (model, _) => model.User.Id, (model, client) => new TransientApplicationTeamMember(client, model) as IApplicationTeamMember);

    private IReadOnlyDictionary<Snowflake, IApplicationTeamMember>? _members;

    /// <inheritdoc cref="INamableEntity.Name"/>
    public string Name => Model.Name;

    /// <inheritdoc/>
    public Snowflake OwnerId => Model.OwnerUserId;

    public TransientApplicationTeam(IClient client, TeamJsonModel model)
        : base(client, model)
    { }
}
