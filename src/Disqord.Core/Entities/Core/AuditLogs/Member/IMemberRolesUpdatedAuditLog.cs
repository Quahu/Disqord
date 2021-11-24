using System.Collections.Generic;

namespace Disqord.AuditLogs
{
    public interface IMemberRolesUpdatedAuditLog : IAuditLog
    {
        /// <summary>
        ///     Gets the names of the roles granted keyed by their IDs.
        /// </summary>
        Optional<IReadOnlyDictionary<Snowflake, string>> RolesGranted { get; }

        /// <summary>
        ///     Gets the names of the roles revoked keyed by their IDs.
        /// </summary>
        Optional<IReadOnlyDictionary<Snowflake, string>> RolesRevoked { get; }

        /// <summary>
        ///     Gets the user this audit log is targeting.
        ///     Returns <see langword="null"/> if the user was not provided with the audit log.
        /// </summary>
        IUser User { get; }
    }
}
