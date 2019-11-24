//using Disqord.Logging;
//using Disqord.Models;

//namespace Disqord.Rest.AuditLogs
//{
//    public sealed class RoleChanges
//    {
//        public AuditLogChange<string> Name { get; }

//        public AuditLogChange<GuildPermissions> Permissions { get; }

//        public AuditLogChange<Color?> Color { get; }

//        public AuditLogChange<bool> IsHoisted { get; }

//        public AuditLogChange<bool> IsMentionable { get; }

//        internal RoleChanges(RestDiscordClient client, AuditLogEntryModel model)
//        {
//            for (var i = 0; i < model.Changes.Length; i++)
//            {
//                var change = model.Changes[i];
//                switch (change.Key)
//                {
//                    case "name":
//                    {
//                        Name = AuditLogChange<string>.SingleConvert(change, client.Serializer);
//                        break;
//                    }

//                    case "permissions":
//                    {
//                        Permissions = AuditLogChange<GuildPermissions>.DoubleConvert<ulong>(change, client.Serializer, x => x);
//                        break;
//                    }

//                    case "color":
//                    {
//                        Color = AuditLogChange<Color?>.DoubleConvert<int>(change, client.Serializer, x => x == 0
//                            ? (int?) null
//                            : x);
//                        break;
//                    }

//                    case "hoist":
//                    {
//                        IsHoisted = AuditLogChange<bool>.SingleConvert(change, client.Serializer);
//                        break;
//                    }

//                    case "mentionable":
//                    {
//                        IsMentionable = AuditLogChange<bool>.SingleConvert(change, client.Serializer);
//                        break;
//                    }

//                    default:
//                    {
//                        client.Log(LogMessageSeverity.Error, $"Unknown change key for {nameof(RoleChanges)}: '{change.Key}'.");
//                        break;
//                    }
//                }
//            }
//        }
//    }
//}
