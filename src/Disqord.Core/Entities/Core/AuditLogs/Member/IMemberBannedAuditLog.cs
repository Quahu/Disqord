namespace Disqord.AuditLogs
{
    public interface IMemberBannedAuditLog : IAuditLog, ITargetedAuditLog<IUser>
    { }
}
