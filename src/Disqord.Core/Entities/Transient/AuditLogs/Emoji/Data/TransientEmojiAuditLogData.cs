using Disqord.Models;
using Qommon;

namespace Disqord.AuditLogs;

public class TransientEmojiAuditLogData : IEmojiAuditLogData
{
    /// <inheritdoc/>
    public Optional<string> Name { get; }

    public TransientEmojiAuditLogData(IClient client, AuditLogEntryJsonModel model, bool isCreated)
    {
        var changes = new TransientEmojiAuditLogChanges(client, model);
        if (isCreated)
        {
            Name = changes.Name.NewValue;
        }
        else
        {
            Name = changes.Name.OldValue;
        }
    }
}
