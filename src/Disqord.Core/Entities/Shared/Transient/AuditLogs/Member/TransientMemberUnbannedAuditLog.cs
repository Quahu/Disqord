﻿using System;
using Disqord.Models;

namespace Disqord.AuditLogs
{
    public class TransientMemberUnbannedAuditLog : TransientAuditLog, IMemberUnbannedAuditLog
    {
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

        public TransientMemberUnbannedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
            : base(client, guildId, auditLogJsonModel, model)
        { }
    }
}
