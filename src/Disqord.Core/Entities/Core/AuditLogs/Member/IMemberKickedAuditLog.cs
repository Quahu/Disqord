namespace Disqord.AuditLogs;

public interface IMemberKickedAuditLog : ITargetedAuditLog<IUser>
{
    /// <summary>
    ///     Gets the type of the integration which kicked the user.
    /// </summary>
    /// <returns>
    ///     The type of the integration or <see langword="null"/> if the user was not kicked by an integration.
    /// </returns>
    string? IntegrationType { get; }
}
