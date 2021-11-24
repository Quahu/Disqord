using System;
using Disqord.Models;

namespace Disqord.AuditLogs
{
    public class TransientMemberUpdatedAuditLog : TransientChangesAuditLog<IMemberAuditLogChanges>, IMemberUpdatedAuditLog
    {
        public override IMemberAuditLogChanges Changes { get; }

        /// <inheritdoc/>
        public IUser User
        {
            get
            {
                if (_user == null)
                {
                    var userModel = Array.Find(AuditLogJsonModel.Users, userModel => userModel.Id == TargetId);
                    if (userModel != null)
                        _user = new TransientUser(Client, userModel);
                }

                return _user;
            }
        }
        private IUser _user;

        public TransientMemberUpdatedAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
            : base(client, guildId, auditLogJsonModel, model)
        {
            Changes = new TransientMemberAuditLogChanges(client, model);
        }
    }
}
