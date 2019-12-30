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
                        Nick = AuditLogChange<string>.SingleConvert(change, client.Serializer);
                        break;
                    }

                    case "deaf":
                    {
                        IsDeafened = AuditLogChange<bool>.SingleConvert(change, client.Serializer);
                        break;
                    }

                    case "mute":
                    {
                        IsMuted = AuditLogChange<bool>.SingleConvert(change, client.Serializer);
                        break;
                    }

                    default:
                    {
                        client.Log(LogMessageSeverity.Error, $"Unknown change key for {nameof(MemberChanges)}: '{change.Key}'.");
                        break;
                    }
                }
            }
        }
    }
}
