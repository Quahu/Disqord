using Disqord.Logging;
using Disqord.Models;

namespace Disqord.Rest.AuditLogs
{
    public sealed class MemberChanges
    {
        public AuditLogChange<string> Nick { get; }

        public AuditLogChange<bool> IsMuted { get; }

        public AuditLogChange<bool> IsDeafened { get; }

        internal MemberChanges(RestDiscordClient client, AuditLogEntryModel model)
        {
            for (var i = 0; i < model.Changes.Length; i++)
            {
                var change = model.Changes[i];
                switch (change.Key)
                {
                    case "nick":
                    {
                        Nick = AuditLogChange<string>.Convert(change);
                        break;
                    }

                    case "deaf":
                    {
                        IsDeafened = AuditLogChange<bool>.Convert(change);
                        break;
                    }

                    case "mute":
                    {
                        IsMuted = AuditLogChange<bool>.Convert(change);
                        break;
                    }

                    default:
                    {
                        client.Log(LogSeverity.Error, $"Unknown change key for {nameof(MemberChanges)}: '{change.Key}'.");
                        break;
                    }
                }
            }
        }
    }
}
