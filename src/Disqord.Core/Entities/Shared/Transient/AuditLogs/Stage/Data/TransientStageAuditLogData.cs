using Disqord.Models;

namespace Disqord.AuditLogs
{
    public class TransientStageAuditLogData : IStageAuditLogData
    {
        public Optional<string> Topic { get; }

        public Optional<PrivacyLevel> PrivacyLevel { get; }

        public TransientStageAuditLogData(IClient client, AuditLogEntryJsonModel model, bool isCreated)
        {
            var changes = new TransientStageAuditLogChanges(client, model);
            if (isCreated)
            {
                Topic = changes.Topic.NewValue;
                PrivacyLevel = changes.PrivacyLevel.NewValue;
            }
            else
            {
                Topic = changes.Topic.OldValue;
                PrivacyLevel = changes.PrivacyLevel.OldValue;
            }
        }
    }
}