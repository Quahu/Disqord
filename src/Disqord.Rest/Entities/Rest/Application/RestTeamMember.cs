using System.Collections.Generic;
using Disqord.Collections;
using Disqord.Models;

namespace Disqord.Rest
{
    public sealed class RestTeamMember : RestUser
    {
        public RestTeam Team { get; }

        public TeamMembershipState MembershipState { get; }

        public IReadOnlyList<string> Permissions { get; }

        internal RestTeamMember(RestTeam team, TeamMemberModel model) : base(team.Client, model.User)
        {
            Team = team;
            MembershipState = model.MembershipState;
            Permissions = model.Permissions.ReadOnly();
        }
    }
}