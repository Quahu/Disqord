using System;
using System.Collections.Generic;
using Qommon.Collections;
using Disqord.Models;
using Microsoft.Extensions.Logging;
using Qommon;
using Qommon.Collections.ReadOnly;

namespace Disqord.AuditLogs
{
    public class TransientMemberRolesUpdatedAuditLog : TransientAuditLog, IMemberRolesUpdatedAuditLog
    {
        /// <inheritdoc/>
        public Optional<IReadOnlyDictionary<Snowflake, string>> GrantedRoles { get; }

        /// <inheritdoc/>
        public Optional<IReadOnlyDictionary<Snowflake, string>> RevokedRoles { get; }

        /// <inheritdoc/>
        public IUser Target
        {
            get
            {
                if (_target == null)
                {
                    var userModel = Array.Find(AuditLogJsonModel.Users, userModel => userModel.Id == TargetId);
                    if (userModel != null)
                        _target = new TransientUser(Client, userModel);
                }

                return _target;
            }
        }
        private IUser _target;

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
                        GrantedRoles = Optional.Convert(change.NewValue, x =>
                        {
                            var models = x.ToType<RoleJsonModel[]>();
                            return models.ToReadOnlyDictionary(x => x.Id, x => x.Name);
                        });
                        break;
                    }
                    case "$remove":
                    {
                        RevokedRoles = Optional.Convert(change.NewValue, x =>
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
