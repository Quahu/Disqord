using Disqord.Models;

namespace Disqord.AuditLogs;

/// <summary>
///     Represents a Discord audit log entry.
/// </summary>
public interface IAuditLog : ISnowflakeEntity, IGuildEntity, IJsonUpdatable<AuditLogEntryJsonModel>
{
    /// <summary>
    ///     Gets the ID of the entity this audit log is targeting.
    /// </summary>
    Snowflake? TargetId { get; }

    /// <summary>
    ///     Gets the ID of the user this audit log was actioned by.
    /// </summary>
    Snowflake? ActorId { get; }

    /// <summary>
    ///     Gets the user this audit log was actioned by.
    ///     Returns <see langword="null"/> if <see cref="ActorId"/> is <see langword="null"/> or if the user was not provided with the audit log.
    /// </summary>
    IUser? Actor { get; }

    /// <summary>
    ///     Gets the reason of this audit log.
    ///     Returns <see langword="null"/> if no reason was provided.
    /// </summary>
    string? Reason { get; }
}
