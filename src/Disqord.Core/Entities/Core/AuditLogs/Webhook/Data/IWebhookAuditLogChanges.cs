namespace Disqord.AuditLogs;

public interface IWebhookAuditLogChanges
{
    AuditLogChange<string> Name { get; }

    AuditLogChange<WebhookType> Type { get; }

    AuditLogChange<string?> AvatarHash { get; }

    AuditLogChange<Snowflake> ChannelId { get; }

    AuditLogChange<Snowflake?> ApplicationId { get; }
}
