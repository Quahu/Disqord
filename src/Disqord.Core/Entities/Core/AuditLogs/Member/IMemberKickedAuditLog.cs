namespace Disqord.AuditLogs
{
    public interface IMemberKickedAuditLog : IAuditLog, ITargetedAuditLog<IUser>
    { }
}
