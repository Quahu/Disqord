using Disqord.Models;

namespace Disqord.AuditLogs
{
    public class TransientRoleAuditLogData : IRoleAuditLogData
    {
        public Optional<string> Name { get; }

        public Optional<GuildPermissions> Permissions { get; }

        public Optional<Color?> Color { get; }

        public Optional<bool> IsHoisted { get; }

        public Optional<bool> IsMentionable { get; }

        public TransientRoleAuditLogData(IClient client, AuditLogEntryJsonModel model, bool isCreated)
        {
            var changes = new TransientRoleAuditLogChanges(client, model);
            if (isCreated)
            {
                Name = changes.Name.NewValue;
                Permissions = changes.Permissions.NewValue;
                Color = changes.Color.NewValue;
                IsHoisted = changes.IsHoisted.NewValue;
                IsMentionable = changes.IsMentionable.NewValue;
            }
            else
            {
                Name = changes.Name.OldValue;
                Permissions = changes.Permissions.OldValue;
                Color = changes.Color.OldValue;
                IsHoisted = changes.IsHoisted.OldValue;
                IsMentionable = changes.IsMentionable.OldValue;
            }
        }
    }
}
