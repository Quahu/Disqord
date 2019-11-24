//using Disqord.Models;

//namespace Disqord.Rest.AuditLogs
//{
//    public sealed class RoleData
//    {
//        public Optional<string> Name { get; }

//        public Optional<GuildPermissions> Permissions { get; }

//        public Optional<Color?> Color { get; }

//        public Optional<bool> IsHoisted { get; }

//        public Optional<bool> IsMentionable { get; }

//        internal RoleData(RestDiscordClient client, AuditLogEntryModel model, bool isCreated)
//        {
//            var changes = new RoleChanges(client, model);
//            if (isCreated)
//            {
//                TargetId = changes.TargetId.NewValue;
//                TargetType = changes.TargetType.NewValue;
//                Allowed = changes.Allowed.NewValue;
//                Denied = changes.Denied.NewValue;
//            }
//            else
//            {
//                TargetId = changes.TargetId.OldValue;
//                TargetType = changes.TargetType.OldValue;
//                Allowed = changes.Allowed.OldValue;
//                Denied = changes.Denied.OldValue;
//            }
//        }
//    }
//}
