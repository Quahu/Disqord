using System;
using Disqord.Models;

namespace Disqord.AuditLogs
{
    public class TransientWebhookUpdatedAuditLog : TransientChangesAuditLog<IWebhookAuditLogChanges>, IWebhookUpdatedAuditLog
    {
        public override IWebhookAuditLogChanges Changes { get; }

        /// <inheritdoc/>
        public IWebhook Webhook
        {
            get
            {
                if (_webhook == null)
                {
                    var webhookModel = Array.Find(AuditLogJsonModel.Webhooks, webhookModel => webhookModel.Id == TargetId);
                    if (webhookModel != null)
                        _webhook = new TransientWebhook(Client, webhookModel);
                }

                return _webhook;
            }
        }
        private IWebhook _webhook;

        public TransientWebhookUpdatedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
            : base(client, guildId, auditLogJsonModel, model)
        {
            Changes = new TransientWebhookAuditLogChanges(client, model);
        }
    }
}
