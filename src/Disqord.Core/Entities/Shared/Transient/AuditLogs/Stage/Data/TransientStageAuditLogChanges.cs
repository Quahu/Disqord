using Disqord.Models;
using Microsoft.Extensions.Logging;

namespace Disqord.AuditLogs
{
    public class TransientStageAuditLogChanges : IStageAuditLogChanges
    {
        public AuditLogChange<string> Topic { get; }

        public AuditLogChange<StagePrivacyLevel> PrivacyLevel { get; }

        public TransientStageAuditLogChanges(IClient client, AuditLogEntryJsonModel model)
        {
            for (var i = 0; i < model.Changes.Value.Length; i++)
            {
                var change = model.Changes.Value[i];
                switch (change.Key)
                {
                    case "topic":
                    {
                        Topic = AuditLogChange<string>.Convert(change);
                        break;
                    }
                    case "privacy_level":
                    {
                        PrivacyLevel = AuditLogChange<StagePrivacyLevel>.Convert(change);
                        break;
                    }
                    default:
                    {
                        client.Logger.LogDebug("Unknown key {0} for {1}", change.Key, this);
                        break;
                    }
                }
            }
        }
    }
}