using Qommon;

namespace Disqord.AuditLogs
{
    public interface IWebhookAuditLogData
    {
        Optional<string> Name { get; }

        Optional<WebhookType> Type { get; }

        Optional<string> AvatarHash { get; }

        Optional<Snowflake> ChannelId { get; }
    }
}
