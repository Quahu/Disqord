using Disqord.Models;
using Microsoft.Extensions.Logging;

namespace Disqord.AuditLogs
{
    public class TransientMemberAuditLogChanges : IMemberAuditLogChanges
    {
        public AuditLogChange<string> Nick { get; }

        public AuditLogChange<bool> IsMuted { get; }

        public AuditLogChange<bool> IsDeafened { get; }

        public TransientMemberAuditLogChanges(IClient client, AuditLogEntryJsonModel model)
        {
            for (var i = 0; i < model.Changes.Value.Length; i++)
            {
                var change = model.Changes.Value[i];
                switch (change.Key)
                {
                    case "nick":
                    {
                        Nick = AuditLogChange<string>.Convert(change);
                        break;
                    }
                    case "mute":
                    {
                        IsMuted = AuditLogChange<bool>.Convert(change);
                        break;
                    }
                    case "deaf":
                    {
                        IsDeafened = AuditLogChange<bool>.Convert(change);
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
