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

    /// <summary>
    ///     Gets the type of the integration which updated the user's roles.
    /// </summary>
    /// <returns>
    ///     The type of the integration or <see langword="null"/> if the roles were not updated by an integration.
    /// </returns>
    string? IntegrationType { get; }
}
