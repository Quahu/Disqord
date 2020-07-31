using System.Collections.Generic;
using Disqord.Collections;
using Disqord.Logging;
using Disqord.Models;

namespace Disqord.Rest.AuditLogs
{
    public sealed class ChannelChanges
    {
        public AuditLogChange<string> Name { get; }

        public AuditLogChange<string> Topic { get; }

        public AuditLogChange<int> Bitrate { get; }

        public AuditLogChange<IReadOnlyList<RestOverwrite>> Overwrites { get; }

        public AuditLogChange<bool> IsNsfw { get; }

        public AuditLogChange<int> Slowmode { get; }

        public AuditLogChange<AuditLogChannelType> Type { get; }

        internal ChannelChanges(RestDiscordClient client, AuditLogEntryModel model)
        {
            for (var i = 0; i < model.Changes.Length; i++)
            {
                var change = model.Changes[i];
                switch (change.Key)
                {
                    case "name":
                    {
                        Name = AuditLogChange<string>.Convert(change);
                        break;
                    }

                    case "topic":
                    {
                        Topic = AuditLogChange<string>.Convert(change);
                        break;
                    }

                    case "bitrate":
                    {
                        Bitrate = AuditLogChange<int>.Convert(change);
                        break;
                    }

                    case "permission_overwrites":
                    {
                        var overwritesBefore = Optional<IReadOnlyList<RestOverwrite>>.Empty;
                        if (change.OldValue.HasValue)
                        {
                            var models = change.OldValue.Value.ToType<OverwriteModel[]>();
                            overwritesBefore = new Optional<IReadOnlyList<RestOverwrite>>(
                                models.ToReadOnlyList((client, model), (x, tuple) =>
                                {
                                    var (client, model) = tuple;
                                    return new RestOverwrite(client, model.TargetId.Value, x);
                                }));
                        }

                        var overwritesAfter = Optional<IReadOnlyList<RestOverwrite>>.Empty;
                        if (change.NewValue.HasValue)
                        {
                            var models = change.NewValue.Value.ToType<OverwriteModel[]>();
                            overwritesAfter = new Optional<IReadOnlyList<RestOverwrite>>(
                                models.ToReadOnlyList((client, model), (x, tuple) =>
                                {
                                    var (client, model) = tuple;
                                    return new RestOverwrite(client, model.TargetId.Value, x);
                                }));
                        }

                        Overwrites = new AuditLogChange<IReadOnlyList<RestOverwrite>>(overwritesBefore, overwritesAfter);
                        break;
                    }

                    case "nsfw":
                    {
                        IsNsfw = AuditLogChange<bool>.Convert(change);
                        break;
                    }

                    case "rate_limit_per_user":
                    {
                        Slowmode = AuditLogChange<int>.Convert(change);
                        break;
                    }

                    case "type":
                    {
                        Type = AuditLogChange<AuditLogChannelType>.Convert(change);
                        break;
                    }

                    default:
                    {
                        client.Log(LogSeverity.Error, $"Unknown change key for {nameof(ChannelChanges)}: '{change.Key}'.");
                        break;
                    }
                }
            }
        }
    }
}
