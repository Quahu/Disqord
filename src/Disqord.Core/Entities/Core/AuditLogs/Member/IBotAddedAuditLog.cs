namespace Disqord.AuditLogs
{
    public interface IBotAddedAuditLog : IAuditLog, ITargetedAuditLog<IUser>
    { }
}
