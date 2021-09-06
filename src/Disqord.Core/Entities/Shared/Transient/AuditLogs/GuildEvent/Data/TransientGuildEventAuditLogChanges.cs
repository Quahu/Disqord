using System.Collections.Generic;
using Disqord.Models;
using Microsoft.Extensions.Logging;

namespace Disqord.AuditLogs
{
    public class TransientGuildEventAuditLogChanges : IGuildEventAuditLogChanges
    {
        public AuditLogChange<IReadOnlyList<Snowflake>> SkuIds { get; }
        public AuditLogChange<GuildEventTarget> EntityType { get; }
        public AuditLogChange<GuildEventStatus> Status { get; }

        public TransientGuildEventAuditLogChanges(IClient client, AuditLogEntryJsonModel model)
        {
            for (var i = 0; i < model.Changes.Value.Length; i++)
            {
                var change = model.Changes.Value[i];
                switch (change.Key)
                {
                    case "sku_ids":
                    {
                        SkuIds = AuditLogChange<IReadOnlyList<Snowflake>>.Convert(change);
                        break;
                    }
                    case "entity_type":
                    {
                        EntityType = AuditLogChange<GuildEventTarget>.Convert(change);
                        break;
                    }
                    case "status":
                    {
                        Status = AuditLogChange<GuildEventStatus>.Convert(change);
                        break;
                    }
                    default:
                    {
                        client.Logger.LogDebug("Unknown key {0} for {1}", change.Key, this);
                        break;
                    }
                }
            }
        }
    }
}