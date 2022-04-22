using Qommon;

namespace Disqord.AuditLogs
{
    public interface IOverwriteAuditLogData
    {
        Optional<Snowflake> TargetId { get; }

        Optional<OverwriteTargetType> TargetType { get; }

        Optional<ChannelPermissions> Allowed { get; }

        Optional<ChannelPermissions> Denied { get; }
    }
}
