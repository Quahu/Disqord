using System;
using Disqord.Models;

namespace Disqord.AuditLogs;

public class TransientThreadCreatedAuditLog : TransientDataAuditLog<IThreadAuditLogData>, IThreadCreatedAuditLog
{
    /// <inheritdoc/>
    public override IThreadAuditLogData Data { get; }

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

    public TransientThreadCreatedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
        : base(client, guildId, auditLogJsonModel, model)
    {
        Data = new TransientThreadAuditLogData(client, model, true);
    }
}
