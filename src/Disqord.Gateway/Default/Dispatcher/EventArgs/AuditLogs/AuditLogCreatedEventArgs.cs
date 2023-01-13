using System;
using Disqord.AuditLogs;

namespace Disqord.Gateway;

public class AuditLogCreatedEventArgs : EventArgs
{
    /// <summary>
    ///     Gets the ID of the guild the audit log was created in.
    /// </summary>
    public Snowflake GuildId { get; }

    /// <summary>
    ///     Gets the created audit log.
    /// </summary>
    /// <remarks>
    ///     Note that the returned audit log will never contain any entities,
    ///     e.g. <see cref="IAuditLog.Actor"/> will always be <see langword="null"/>.
    /// </remarks>
    public IAuditLog AuditLog { get; }

    public AuditLogCreatedEventArgs(
        Snowflake guildId,
        IAuditLog auditLog)
    {
        GuildId = guildId;
        AuditLog = auditLog;
    }
}
