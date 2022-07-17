using System.Collections.Generic;
using Disqord.Models;

namespace Disqord;

/// <summary>
///     Represents a member of a team of a Discord application.
/// </summary>
public interface IApplicationTeamMember : IUser, IJsonUpdatable<TeamMemberJsonModel>
{
    /// <summary>
    ///     Gets the ID of the team of this member.
    /// </summary>
    Snowflake TeamId { get; }

    /// <summary>
    ///     Gets the membership state of this member.
    /// </summary>
    TeamMembershipState MembershipState { get; }

    /// <summary>
    ///     Gets the permissions of this member.
    /// </summary>
    IReadOnlyList<string> Permissions { get; }
}