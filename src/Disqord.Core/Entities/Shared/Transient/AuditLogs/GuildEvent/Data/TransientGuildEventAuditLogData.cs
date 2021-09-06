using System.Collections.Generic;
using Disqord.Models;

namespace Disqord.AuditLogs
{
    public class TransientGuildEventAuditLogData : IGuildEventAuditLogData
    {
        public Optional<IReadOnlyList<Snowflake>> SkuIds { get; }
        public Optional<GuildEventTarget> EntityType { get; }
        public Optional<GuildEventStatus> Status { get; }

        public TransientGuildEventAuditLogData(IClient client, AuditLogEntryJsonModel model, bool isCreated)
        {
            var changes = new TransientGuildEventAuditLogChanges(client, model);
            if (isCreated)
            {
                SkuIds = changes.SkuIds.NewValue;
                EntityType = changes.EntityType.NewValue;
                Status = changes.Status.NewValue;
            }
            else
            {
                SkuIds = changes.SkuIds.OldValue;
                EntityType = changes.EntityType.OldValue;
                Status = changes.Status.OldValue;
            }
        }
    }
}