using Disqord.Logging;
using Disqord.Models;

namespace Disqord.Rest.AuditLogs
{
    public sealed class OverwriteChanges
    {
        public AuditLogChange<Snowflake> TargetId { get; }

        public AuditLogChange<OverwriteTargetType> TargetType { get; }

        public AuditLogChange<ChannelPermissions> Allowed { get; }

        public AuditLogChange<ChannelPermissions> Denied { get; }

        internal OverwriteChanges(RestDiscordClient client, AuditLogEntryModel model)
        {
            for (var i = 0; i < model.Changes.Length; i++)
            {
                var change = model.Changes[i];
                switch (change.Key)
                {
                    case "id":
                    {
                        TargetId = AuditLogChange<Snowflake>.Convert(change);
                        break;
                    }

                    case "allow":
                    {
                        Allowed = AuditLogChange<ChannelPermissions>.Convert<ulong>(change, x => x);
                        break;
                    }

                    case "deny":
                    {
                        Denied = AuditLogChange<ChannelPermissions>.Convert<ulong>(change, x => x);
                        break;
                    }

                    case "type":
                    {
                        TargetType = AuditLogChange<OverwriteTargetType>.Convert(change);
                        break;
                    }

                    default:
                    {
                        client.Log(LogSeverity.Error, $"Unknown change key for {nameof(OverwriteChanges)}: '{change.Key}'.");
                        break;
                    }
                }
            }
        }
    }
}
