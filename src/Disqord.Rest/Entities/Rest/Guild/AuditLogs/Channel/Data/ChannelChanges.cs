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
                        Name = AuditLogChange<string>.SingleConvert(change, client.Serializer);
                        break;
                    }

                    case "topic":
                    {
                        Topic = AuditLogChange<string>.SingleConvert(change, client.Serializer);
                        break;
                    }

                    case "bitrate":
                    {
                        Bitrate = AuditLogChange<int>.SingleConvert(change, client.Serializer);
                        break;
                    }

                    case "permission_overwrites":
                    {
                        var overwritesBefore = Optional<IReadOnlyList<RestOverwrite>>.Empty;
                        if (change.OldValue.HasValue)
                        {
                            var models = client.Serializer.ToObject<OverwriteModel[]>(change.OldValue.Value);
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
                            var models = client.Serializer.ToObject<OverwriteModel[]>(change.NewValue.Value);
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
                        IsNsfw = AuditLogChange<bool>.SingleConvert(change, client.Serializer);
                        break;
                    }

                    case "rate_limit_per_user":
                    {
                        Slowmode = AuditLogChange<int>.SingleConvert(change, client.Serializer);
                        break;
                    }

                    case "type":
                    {
                        Type = AuditLogChange<AuditLogChannelType>.SingleConvert(change, client.Serializer);
                        break;
                    }

                    default:
                    {
                        client.Log(LogMessageSeverity.Error, $"Unknown change key for {nameof(ChannelChanges)}: '{change.Key}'.");
                        break;
                    }
                }
            }
        }
    }
}
