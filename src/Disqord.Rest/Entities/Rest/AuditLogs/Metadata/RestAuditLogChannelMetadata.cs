using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Disqord.Models;

namespace Disqord.Rest.AuditLogs
{
    public sealed class RestAuditLogChannelMetadata : RestAuditLogMetadata
    {
        public AuditLogChannelType ChannelType { get; }

        public Snowflake ChannelId { get; }

        public AuditLogValue<string> Name { get; }

        public AuditLogValue<string> Topic { get; }

        public AuditLogValue<int> Bitrate { get; }

        public AuditLogValue<IReadOnlyList<RestOverwrite>> Overwrites { get; }

        public AuditLogValue<bool> IsNsfw { get; }

        public AuditLogValue<int> Slowmode { get; }

        internal RestAuditLogChannelMetadata(RestDiscordClient client, AuditLogModel auditLogModel, AuditLogEntryModel model) : base(client)
        {
            ChannelId = model.TargetId;

            for (var i = 0; i < model.Changes.Length; i++)
            {
                var change = model.Changes[i];
                switch (change.Key)
                {
                    case "name":
                    {
                        Name = new AuditLogValue<string>(change);
                        break;
                    }

                    case "topic":
                    {
                        Topic = new AuditLogValue<string>(change);
                        break;
                    }

                    case "bitrate":
                    {
                        Bitrate = new AuditLogValue<int>(change);
                        break;
                    }

                    case "permission_overwrites":
                    {
                        var overwritesBefore = Optional<IReadOnlyList<RestOverwrite>>.Empty;
                        if (change.OldValue.HasValue)
                        {
                            var models = client.Serializer.ToObject<OverwriteModel[]>(change.OldValue.Value);
                            overwritesBefore = models.Select(x => new RestOverwrite(client, x, ChannelId)).ToImmutableArray();
                        }

                        var overwritesAfter = Optional<IReadOnlyList<RestOverwrite>>.Empty;
                        if (change.NewValue.HasValue)
                        {
                            var models = client.Serializer.ToObject<OverwriteModel[]>(change.NewValue.Value);
                            overwritesAfter = models.Select(x => new RestOverwrite(client, x, ChannelId)).ToImmutableArray();
                        }

                        Overwrites = new AuditLogValue<IReadOnlyList<RestOverwrite>>(overwritesBefore, overwritesAfter);
                        break;
                    }

                    case "nsfw":
                    {
                        IsNsfw = new AuditLogValue<bool>(change);
                        break;
                    }

                    case "rate_limit_per_user":
                    {
                        Slowmode = new AuditLogValue<int>(change);
                        break;
                    }

                    case "type":
                    {
                        ChannelType = client.Serializer.ToObject<AuditLogChannelType>(change.OldValue.HasValue ? change.OldValue.Value : change.NewValue.Value);
                        break;
                    }
                }
            }
        }
    }
}
