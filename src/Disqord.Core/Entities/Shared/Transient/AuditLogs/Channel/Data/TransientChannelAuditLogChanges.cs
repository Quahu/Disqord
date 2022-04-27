using System;
using System.Collections.Generic;
using Qommon.Collections;
using Disqord.Models;
using Microsoft.Extensions.Logging;
using Qommon.Collections.ReadOnly;

namespace Disqord.AuditLogs
{
    public class TransientChannelAuditLogChanges : IChannelAuditLogChanges
    {
        public AuditLogChange<string> Name { get; }

        public AuditLogChange<string> Topic { get; }

        public AuditLogChange<int> Bitrate { get; }

        public AuditLogChange<int> MemberLimit { get; }

        public AuditLogChange<IReadOnlyList<IOverwrite>> Overwrites { get; }

        public AuditLogChange<bool> IsNsfw { get; }

        public AuditLogChange<TimeSpan> Slowmode { get; }

        public AuditLogChange<ChannelType> Type { get; }

        public AuditLogChange<string> Region { get; }

        public TransientChannelAuditLogChanges(IClient client, AuditLogEntryJsonModel model)
        {
            for (var i = 0; i < model.Changes.Value.Length; i++)
            {
                var change = model.Changes.Value[i];
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
                    case "user_limit":
                    {
                        MemberLimit = AuditLogChange<int>.Convert(change);
                        break;
                    }
                    case "permission_overwrites":
                    {
                        Overwrites = AuditLogChange<IReadOnlyList<IOverwrite>>.Convert(change, (client, model.TargetId.Value),
                            (OverwriteJsonModel[] x, (IClient, Snowflake) tuple) => x.ToReadOnlyList(tuple, (x, tuple) =>
                            {
                                var (client, channelId) = tuple;
                                return new TransientOverwrite(client, channelId, x);
                            }));
                        break;
                    }
                    case "nsfw":
                    {
                        IsNsfw = AuditLogChange<bool>.Convert(change);
                        break;
                    }
                    case "rate_limit_per_user":
                    {
                        Slowmode = AuditLogChange<TimeSpan>.Convert<int>(change, x => TimeSpan.FromSeconds(x));
                        break;
                    }
                    case "type":
                    {
                        Type = AuditLogChange<ChannelType>.Convert(change);
                        break;
                    }
                    case "rtc_region":
                    {
                        Region = AuditLogChange<string>.Convert(change);
                        break;
                    }
                    default:
                    {
                        client.Logger.LogDebug("Unknown key {0} for {1}", change.Key, this);
                        break;
                    }
                }
            }
        }
    }
}
