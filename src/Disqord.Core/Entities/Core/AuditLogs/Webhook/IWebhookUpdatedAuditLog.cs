﻿namespace Disqord.AuditLogs
{
    public interface IWebhookUpdatedAuditLog : IChangesAuditLog<IWebhookAuditLogChanges>
    {
        /// <summary>
        ///     Gets the webhook this audit log is targeting.
        ///     Returns <see langword="null"/> if the webhook was not provided with the audit log.
        /// </summary>
        IWebhook Webhook { get; }
    }
}
