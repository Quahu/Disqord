using System.Collections.Generic;
using Disqord.Collections;
using Disqord.Models;
using Microsoft.Extensions.Logging;

namespace Disqord.AuditLogs
{
    public class TransientMemberRolesUpdatedAuditLog : TransientAuditLog, IMemberRolesUpdatedAuditLog
    {
        public Optional<IReadOnlyDictionary<Snowflake, string>> RolesGranted { get; }

        public Optional<IReadOnlyDictionary<Snowflake, string>> RolesRevoked { get; }

        public TransientMemberRolesUpdatedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
            : base(client, guildId, auditLogJsonModel, model)
        {
            for (var i = 0; i < model.Changes.Value.Length; i++)
            {
                var change = model.Changes.Value[i];
                switch (change.Key)
                {
                    case "$add":
                    {
                        RolesGranted = Optional.Convert(change.NewValue, x =>
                        {
                            var models = x.ToType<RoleJsonModel[]>();
                            return models.ToReadOnlyDictionary(x => x.Id, x => x.Name);
                        });
                        break;
                    }
                    case "$remove":
                    {
                        RolesRevoked = Optional.Convert(change.NewValue, x =>
                        {
                            var models = x.ToType<RoleJsonModel[]>();
                            return models.ToReadOnlyDictionary(x => x.Id, x => x.Name);
                        });
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
