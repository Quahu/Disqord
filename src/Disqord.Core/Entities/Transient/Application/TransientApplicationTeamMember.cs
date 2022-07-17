using System.Collections.Generic;
using Disqord.Models;

namespace Disqord;

/// <inheritdoc cref="IApplicationTeamMember"/>
public class TransientApplicationTeamMember : TransientUser, IApplicationTeamMember, ITransientEntity<TeamMemberJsonModel>
{
    /// <inheritdoc/>
    public Snowflake TeamId => Model.TeamId;

    /// <inheritdoc/>
    public TeamMembershipState MembershipState => Model.MembershipState;

    /// <inheritdoc/>
    public IReadOnlyList<string> Permissions => Model.Permissions;

    /// <inheritdoc/>
    public new TeamMemberJsonModel Model { get; }

    public TransientApplicationTeamMember(IClient client, TeamMemberJsonModel model)
        : base(client, model.User)
    {
        Model = model;
    }
}