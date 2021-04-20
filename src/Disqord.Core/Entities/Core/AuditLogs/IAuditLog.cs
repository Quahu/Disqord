namespace Disqord.AuditLogs
{
    /// <summary>
    ///     Represents a Discord audit log entry.
    /// </summary>
    public interface IAuditLog : ISnowflakeEntity, IGuildEntity
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
        ///     Gets the optional user this audit log was actioned by.
        /// </summary>
        Optional<IUser> Actor { get; }
        
        /// <summary>
        ///     Gets the reason of this audit log.
        /// </summary>
        string Reason { get; }
    }
}
