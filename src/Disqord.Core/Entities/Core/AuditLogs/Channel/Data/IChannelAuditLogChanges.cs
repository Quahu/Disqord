using System.Collections.Generic;

namespace Disqord.AuditLogs
{
    public interface IChannelAuditLogChanges
    {
        AuditLogChange<string> Name { get; }

        AuditLogChange<string> Topic { get; }

        AuditLogChange<int> Bitrate { get; }

        AuditLogChange<IReadOnlyList<IOverwrite>> Overwrites { get; }

        AuditLogChange<bool> IsNsfw { get; }

        AuditLogChange<int> Slowmode { get; }

        AuditLogChange<ChannelType> Type { get; }
    }
}
