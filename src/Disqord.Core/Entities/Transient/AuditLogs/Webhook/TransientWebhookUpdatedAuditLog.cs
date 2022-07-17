using System;
using Disqord.Models;

namespace Disqord.AuditLogs;

public class TransientWebhookUpdatedAuditLog : TransientChangesAuditLog<IWebhookAuditLogChanges>, IWebhookUpdatedAuditLog
{
    /// <inheritdoc/>
    public override IWebhookAuditLogChanges Changes { get; }

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

    public TransientWebhookUpdatedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
        : base(client, guildId, auditLogJsonModel, model)
    {
        Changes = new TransientWebhookAuditLogChanges(client, model);
    }
}
