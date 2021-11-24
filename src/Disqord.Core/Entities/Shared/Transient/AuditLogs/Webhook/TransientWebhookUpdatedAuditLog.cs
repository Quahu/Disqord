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
                    var webhook = Array.Find(AuditLogJsonModel.Webhooks, x => x.Id == TargetId);
                    if (webhook != null)
                        _webhook = new TransientWebhook(Client, webhook);
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
