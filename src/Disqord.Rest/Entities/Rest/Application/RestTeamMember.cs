using System.Collections.Generic;
using System.Collections.Immutable;
using Disqord.Models;

namespace Disqord.Rest
{
    public sealed class RestTeamMember : RestUser
    {
        public TeamMembershipState MembershipState { get; }

        public IReadOnlyList<string> Permissions { get; }

        internal RestTeamMember(RestDiscordClient client, TeamMemberModel model) : base(client, model)
        {
            MembershipState = model.MembershipState;
            Permissions = model.Permissions.ToImmutableArray();
        }
    }
}