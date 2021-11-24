using System;
using Disqord.Models;

namespace Disqord.AuditLogs
{
    public class TransientWebhookCreatedAuditLog : TransientDataAuditLog<IWebhookAuditLogData>, IWebhookCreatedAuditLog
    {
        public override IWebhookAuditLogData Data { get; }

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

        public TransientWebhookCreatedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
            : base(client, guildId, auditLogJsonModel, model)
        {
            Data = new TransientWebhookAuditLogData(client, model, true);
        }
    }
}
