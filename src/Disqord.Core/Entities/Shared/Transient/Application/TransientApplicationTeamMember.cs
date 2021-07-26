using System.Collections.Generic;
using Disqord.Models;

namespace Disqord
{
    /// <inheritdoc cref="IApplicationTeamMember"/>
    public class TransientApplicationTeamMember : TransientUser, IApplicationTeamMember
    {
        /// <inheritdoc/>
        public Snowflake TeamId => Model.TeamId;

        /// <inheritdoc/>
        public TeamMembershipState MembershipState => Model.MembershipState;

        /// <inheritdoc/>
        public IReadOnlyList<string> Permissions => Model.Permissions;

        /// <inheritdoc cref="ITransientEntity{TModel}.Model"/>
        public new TeamMemberJsonModel Model { get; }

        public TransientApplicationTeamMember(IClient client, TeamMemberJsonModel model)
            : base(client, model.User)
        {
            Model = model;
        }
    }
}
