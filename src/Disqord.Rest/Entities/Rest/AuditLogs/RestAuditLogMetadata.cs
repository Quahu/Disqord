namespace Disqord.Rest.AuditLogs
{
    public abstract class RestAuditLogMetadata : RestDiscordEntity
    {
        internal RestAuditLogMetadata(RestDiscordClient client) : base(client)
        { }
    }
}
