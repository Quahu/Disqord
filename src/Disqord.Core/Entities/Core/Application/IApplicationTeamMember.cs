using System.Collections.Generic;

namespace Disqord
{
    public interface IApplicationTeamMember : IUser
    {
        Snowflake TeamId { get; }

        TeamMembershipState MembershipState { get; }

        IReadOnlyList<string> Permissions { get; }
    }
}
