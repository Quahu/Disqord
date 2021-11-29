namespace Disqord.AuditLogs
{
    public interface IMemberUnbannedAuditLog : IAuditLog, ITargetedAuditLog<IUser>
    { }
}
