using System.Collections.Generic;
using Qommon;

namespace Disqord.AuditLogs;

public interface IMemberRolesUpdatedAuditLog : ITargetedAuditLog<IUser>
{
    /// <summary>
    ///     Gets the names of the roles granted keyed by their IDs.
    /// </summary>
    Optional<IReadOnlyDictionary<Snowflake, string>> GrantedRoles { get; }

    /// <summary>
    ///     Gets the names of the roles revoked keyed by their IDs.
    /// </summary>
    Optional<IReadOnlyDictionary<Snowflake, string>> RevokedRoles { get; }
}