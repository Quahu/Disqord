using System;
using Disqord.Models;

namespace Disqord.AuditLogs;

public class TransientWebhookCreatedAuditLog : TransientDataAuditLog<IWebhookAuditLogData>, IWebhookCreatedAuditLog
{
    /// <inheritdoc/>
    public override IWebhookAuditLogData Data { get; }

    /// <inheritdoc/>
    public IWebhook? Target
    {
        get
        {
            if (_target == null)
            {
                var webhookModel = Array.Find(AuditLogJsonModel.Webhooks, webhookModel => webhookModel.Id == TargetId);
                if (webhookModel != null)
                    _target = new TransientWebhook(Client, webhookModel);
            }

            return _target;
        }
    }
    private IWebhook? _target;

    public TransientWebhookCreatedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
        : base(client, guildId, auditLogJsonModel, model)
    {
        Data = new TransientWebhookAuditLogData(client, model, true);
    }
}
