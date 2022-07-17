using Disqord.Models;
using Microsoft.Extensions.Logging;

namespace Disqord.AuditLogs;

public class TransientEmojiAuditLogChanges : IEmojiAuditLogChanges
{
    /// <inheritdoc/>
    public AuditLogChange<string> Name { get; }

    public TransientEmojiAuditLogChanges(IClient client, AuditLogEntryJsonModel model)
    {
        for (var i = 0; i < model.Changes.Value.Length; i++)
        {
            var change = model.Changes.Value[i];
            switch (change.Key)
            {
                case "name":
                {
                    Name = AuditLogChange<string>.Convert(change);
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
