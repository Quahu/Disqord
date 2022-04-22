using System;
using System.Collections.Generic;
using Qommon;

namespace Disqord.AuditLogs
{
    public interface IChannelAuditLogData
    {
        Optional<string> Name { get; }

        Optional<string> Topic { get; }

        Optional<int> Bitrate { get; }

        Optional<IReadOnlyList<IOverwrite>> Overwrites { get; }

        Optional<bool> IsNsfw { get; }

        Optional<TimeSpan> Slowmode { get; }

        Optional<ChannelType> Type { get; }
    }
}
