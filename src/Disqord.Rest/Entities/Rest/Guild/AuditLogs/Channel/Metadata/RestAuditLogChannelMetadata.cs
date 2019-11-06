//using System.Collections.Generic;
//using System.Collections.Immutable;
//using System.Linq;
//using Disqord.Models;

//namespace Disqord.Rest.AuditLogs
//{
//    public sealed class RestAuditLogChannelMetadata : RestAuditLogMetadata
//    {
//        public RestAuditLog AuditLog { get; }

//        public AuditLogValue<AuditLogChannelType> Type { get; }

//        public AuditLogValue<string> Name { get; }

//        public AuditLogValue<string> Topic { get; }

//        public AuditLogValue<int> Bitrate { get; }

//        public AuditLogValue<IReadOnlyList<RestOverwrite>> Overwrites { get; }

//        public AuditLogValue<bool> IsNsfw { get; }

//        public AuditLogValue<int> Slowmode { get; }

//        internal RestAuditLogChannelMetadata(RestAuditLog auditLog, AuditLogModel auditLogModel, AuditLogEntryModel model) : base(auditLog.Client)
//        {
//            AuditLog = auditLog;
//            for (var i = 0; i < model.Changes.Length; i++)
//            {
//                var change = model.Changes[i];
//                switch (change.Key)
//                {
//                    case "name":
//                    {
//                        Name = new AuditLogValue<string>(change);
//                        break;
//                    }

//                    case "topic":
//                    {
//                        Topic = new AuditLogValue<string>(change);
//                        break;
//                    }

//                    case "bitrate":
//                    {
//                        Bitrate = new AuditLogValue<int>(change);
//                        break;
//                    }

//                    case "permission_overwrites":
//                    {
//                        var overwritesBefore = Optional<IReadOnlyList<RestOverwrite>>.Empty;
//                        if (change.OldValue.HasValue)
//                        {
//                            var models = Client.Serializer.ToObject<OverwriteModel[]>(change.OldValue.Value);
//                            overwritesBefore = models.Select(x => new RestOverwrite(Client, model.TargetId, x)).ToImmutableArray();
//                        }

//                        var overwritesAfter = Optional<IReadOnlyList<RestOverwrite>>.Empty;
//                        if (change.NewValue.HasValue)
//                        {
//                            var models = Client.Serializer.ToObject<OverwriteModel[]>(change.NewValue.Value);
//                            overwritesAfter = models.Select(x => new RestOverwrite(Client, model.TargetId, x)).ToImmutableArray();
//                        }

//                        Overwrites = new AuditLogValue<IReadOnlyList<RestOverwrite>>(overwritesBefore, overwritesAfter);
//                        break;
//                    }

//                    case "nsfw":
//                    {
//                        IsNsfw = new AuditLogValue<bool>(change);
//                        break;
//                    }

//                    case "rate_limit_per_user":
//                    {
//                        Slowmode = new AuditLogValue<int>(change);
//                        break;
//                    }

//                    case "type":
//                    {
//                        Type = new AuditLogValue<AuditLogChannelType>(change, x => Client.Serializer.ToObject<AuditLogChannelType>(x));
//                        break;
//                    }
//                }
//            }
//        }
//    }
//}
