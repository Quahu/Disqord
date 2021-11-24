using System;
using System.Collections.Generic;
using Qommon.Collections;
using Disqord.Models;
using Microsoft.Extensions.Logging;

namespace Disqord.AuditLogs
{
    public class TransientMemberRolesUpdatedAuditLog : TransientAuditLog, IMemberRolesUpdatedAuditLog
    {
        /// <inheritdoc/>
        public Optional<IReadOnlyDictionary<Snowflake, string>> RolesGranted { get; }

        /// <inheritdoc/>
        public Optional<IReadOnlyDictionary<Snowflake, string>> RolesRevoked { get; }

        /// <inheritdoc/>
        public IUser User
        {
            get
            {
                if (_user == null)
                {
                    var user = Array.Find(AuditLogJsonModel.Users, x => x.Id == TargetId);
                    if (user != null)
                        _user = new TransientUser(Client, user);
                }

                return _user;
            }
        }
        private IUser _user;

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
