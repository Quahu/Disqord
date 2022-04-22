using Disqord.Models;
using Qommon;

namespace Disqord.AuditLogs
{
    public class TransientOverwriteAuditLogData : IOverwriteAuditLogData
    {
        public Optional<Snowflake> TargetId { get; }

        public Optional<OverwriteTargetType> TargetType { get; }

        public Optional<ChannelPermissions> Allowed { get; }

        public Optional<ChannelPermissions> Denied { get; }

        public TransientOverwriteAuditLogData(IClient client, AuditLogEntryJsonModel model, bool isCreated)
        {
            var changes = new TransientOverwriteAuditLogChanges(client, model);
            if (isCreated)
            {
                TargetId = changes.TargetId.NewValue;
                TargetType = changes.TargetType.NewValue;
                Allowed = changes.Allowed.NewValue;
                Denied = changes.Denied.NewValue;
            }
            else
            {
                TargetId = changes.TargetId.OldValue;
                TargetType = changes.TargetType.OldValue;
                Allowed = changes.Allowed.OldValue;
                Denied = changes.Denied.OldValue;
            }
        }
    }
}
