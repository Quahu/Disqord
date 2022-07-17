namespace Disqord.AuditLogs;

public interface IEmojiAuditLogChanges
{
    AuditLogChange<string> Name { get; }
}