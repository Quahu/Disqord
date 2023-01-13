namespace Disqord.AuditLogs;

public interface IOverwriteAuditLog
{
    /// <summary>
    ///     Gets the ID of the target entity of the overwrite.
    /// </summary>
    Snowflake OverwriteTargetId { get; }

    /// <summary>
    ///     Gets the target type of the overwrite.
    /// </summary>
    OverwriteTargetType OverwriteTargetType { get; }

    /// <summary>
    ///     Gets the name of the target role of the overwrite.
    /// </summary>
    /// <returns>
    ///     The name of the role or <see langword="null"/> if the target type is not <see cref="Disqord.OverwriteTargetType.Role"/>.
    /// </returns>
    string? OverwriteTargetRoleName { get; }
}
