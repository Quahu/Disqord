using Disqord.Models;

namespace Disqord.Rest.AuditLogs
{
    public sealed class OverwriteData
    {
        public Optional<Snowflake> TargetId { get; }

        public Optional<OverwriteTargetType> TargetType { get; }

        public Optional<ChannelPermissions> Allowed { get; }

        public Optional<ChannelPermissions> Denied { get; }

        internal OverwriteData(RestDiscordClient client, AuditLogEntryModel model, bool isCreated)
        {
            var changes = new OverwriteChanges(client, model);
            if (isCreated)
            {
                TargetId = changes.TargetId.NewValue;
                TargetType = changes.TargetType.NewValue;
                Allowed = changes.Allowed.NewValue;
                Denied = changes.Denied.NewValue;
            }
            else
            {
                TargetId = changes.TargetId.OldValue;
                TargetType = changes.TargetType.OldValue;
                Allowed = changes.Allowed.OldValue;
                Denied = changes.Denied.OldValue;
            }
        }
    }
}
