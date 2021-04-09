using System.Collections.Generic;
using Disqord.Rest;

namespace Disqord
{
    public interface IApplicationTeamMember : IUser
    {
        Snowflake TeamId { get; }
        
        TeamMembershipState MembershipState { get; }
        
        IReadOnlyList<string> Permissions { get; }
    }
}
