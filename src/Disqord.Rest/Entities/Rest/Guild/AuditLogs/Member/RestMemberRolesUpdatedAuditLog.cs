using System.Collections.Generic;
using System.Linq;
using Disqord.Collections;
using Disqord.Logging;
using Disqord.Models;

namespace Disqord.Rest.AuditLogs
{
    public sealed class RestMemberRolesUpdatedAuditLog : RestAuditLog
    {
        public Optional<IReadOnlyList<Role>> RolesAdded { get; }

        public Optional<IReadOnlyList<Role>> RolesRemoved { get; }

        internal RestMemberRolesUpdatedAuditLog(RestDiscordClient client, AuditLogModel log, AuditLogEntryModel entry) : base(client, log, entry)
        {
            for (var i = 0; i < entry.Changes.Length; i++)
            {
                var change = entry.Changes[i];
                switch (change.Key)
                {
                    case "$add":
                    {
                        RolesAdded = AuditLogChange<IReadOnlyList<Role>>.DoubleConvert<RoleModel[]>(
                            change, client.Serializer, x => x.ToReadOnlyList(y => new Role(y))).NewValue;
                        break;
                    }

                    case "$remove":
                    {
                        RolesRemoved = AuditLogChange<IReadOnlyList<Role>>.DoubleConvert<RoleModel[]>(
                            change, client.Serializer, x => x.ToReadOnlyList(y => new Role(y))).NewValue;
                        break;
                    }

                    default:
                    {
                        client.Log(LogMessageSeverity.Error, $"Unknown change key for {nameof(RestMemberRolesUpdatedAuditLog)}: '{change.Key}'.");
                        break;
                    }
                }
            }
        }

        public sealed class Role
        {
            public Snowflake Id { get; }

            public string Name { get; }

            internal Role(RoleModel model)
            {
                Id = model.Id;
                Name = model.Name;
            }
        }
    }
}
