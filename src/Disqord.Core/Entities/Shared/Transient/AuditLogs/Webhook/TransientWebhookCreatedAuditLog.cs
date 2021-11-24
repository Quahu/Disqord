using System;
using Disqord.Models;

namespace Disqord.AuditLogs
{
    public class TransientWebhookCreatedAuditLog : TransientDataAuditLog<IWebhookAuditLogData>, IWebhookCreatedAuditLog
    {
        /// <inheritdoc/>
        public override IWebhookAuditLogData Data { get; }

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

        public TransientWebhookCreatedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
            : base(client, guildId, auditLogJsonModel, model)
        {
            Data = new TransientWebhookAuditLogData(client, model, true);
        }
    }
}
