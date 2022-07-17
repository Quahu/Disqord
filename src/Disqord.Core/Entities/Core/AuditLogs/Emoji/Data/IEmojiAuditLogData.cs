using Qommon;

namespace Disqord.AuditLogs;

public interface IEmojiAuditLogData
{
    Optional<string> Name { get; }
}