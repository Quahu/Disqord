using System;
using Disqord.Models;

namespace Disqord.AuditLogs;

public class TransientThreadUpdatedAuditLog : TransientChangesAuditLog<IThreadAuditLogChanges>, IThreadUpdatedAuditLog
{
    /// <inheritdoc/>
    public override IThreadAuditLogChanges Changes { get; }

    /// <inheritdoc/>
    public IThreadChannel? Target
    {
        get
        {
            if (_target == null)
            {
                var threadModel = Array.Find(AuditLogJsonModel.Threads, threadModel => threadModel.Id == TargetId);
                if (threadModel != null)
                    _target = new TransientThreadChannel(Client, threadModel);
            }

            return _target;
        }
    }
    private IThreadChannel? _target;

    public TransientThreadUpdatedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
        : base(client, guildId, auditLogJsonModel, model)
    {
        Changes = new TransientThreadAuditLogChanges(client, model);
    }
}
