using Disqord.Models;

namespace Disqord.Rest.AuditLogs
{
    public sealed class RestUnknownAuditLog : RestAuditLog
    {
        public int Type { get; }

        internal RestUnknownAuditLog(RestDiscordClient client, AuditLogModel model, AuditLogEntryModel entryModel) : base(client, model, entryModel)
        {
            Type = (int) entryModel.ActionType;
        }
    }
}
