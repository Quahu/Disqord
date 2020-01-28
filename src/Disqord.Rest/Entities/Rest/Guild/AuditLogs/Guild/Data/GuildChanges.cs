using System;
using Disqord.Logging;
using Disqord.Models;

namespace Disqord.Rest.AuditLogs
{
    public sealed class GuildChanges
    {
        public AuditLogChange<string> Name { get; }

        public AuditLogChange<string> IconHash { get; }

        public AuditLogChange<string> SplashHash { get; }

        public AuditLogChange<Snowflake> OwnerId { get; }

        public AuditLogChange<SnowflakeOptional<RestUser>> Owner { get; }

        public AuditLogChange<string> VoiceRegionId { get; }

        public AuditLogChange<Snowflake?> AfkChannelId { get; }

        public AuditLogChange<int> AfkTimeout { get; }

        public AuditLogChange<MfaLevel> MfaLevel { get; }

        public AuditLogChange<VerificationLevel> VerificationLevel { get; }

        public AuditLogChange<ContentFilterLevel> ContentFilterLevel { get; }

        public AuditLogChange<DefaultNotificationLevel> DefaultNotificationLevel { get; }

        public AuditLogChange<string> VanityUrlCode { get; }

        public AuditLogChange<bool> IsWidgetEnabled { get; }

        public AuditLogChange<Snowflake?> WidgetChannelId { get; }

        public AuditLogChange<Snowflake?> SystemChannelId { get; }

        internal GuildChanges(RestDiscordClient client, AuditLogModel log, AuditLogEntryModel entry)
        {
            for (var i = 0; i < entry.Changes.Length; i++)
            {
                var change = entry.Changes[i];
                switch (change.Key)
                {
                    case "name":
                    {
                        Name = AuditLogChange<string>.SingleConvert(change, client.Serializer);
                        break;
                    }

                    case "icon_hash":
                    {
                        IconHash = AuditLogChange<string>.SingleConvert(change, client.Serializer);
                        break;
                    }

                    case "splash_hash":
                    {
                        SplashHash = AuditLogChange<string>.SingleConvert(change, client.Serializer);
                        break;
                    }

                    case "owner_id":
                    {
                        OwnerId = AuditLogChange<Snowflake>.DoubleConvert<ulong>(change, client.Serializer, x => x);
                        var ownerBeforeModel = Array.Find(log.Users, x => x.Id == OwnerId.OldValue.Value);
                        var ownerAfterModel = Array.Find(log.Users, x => x.Id == OwnerId.NewValue.Value);
                        Owner = new AuditLogChange<SnowflakeOptional<RestUser>>(
                            ownerBeforeModel != null
                                ? new SnowflakeOptional<RestUser>(new RestUser(client, ownerBeforeModel))
                                : new SnowflakeOptional<RestUser>(OwnerId.OldValue.Value),
                            ownerAfterModel != null
                                ? new SnowflakeOptional<RestUser>(new RestUser(client, ownerAfterModel))
                                : new SnowflakeOptional<RestUser>(OwnerId.NewValue.Value));
                        break;
                    }

                    case "region":
                    {
                        VoiceRegionId = AuditLogChange<string>.SingleConvert(change, client.Serializer);
                        break;
                    }

                    case "afk_channel_id":
                    {
                        AfkChannelId = AuditLogChange<Snowflake?>.DoubleConvert<ulong?>(change, client.Serializer, x => x);
                        break;
                    }

                    case "afk_timeout":
                    {
                        AfkTimeout = AuditLogChange<int>.SingleConvert(change, client.Serializer);
                        break;
                    }

                    case "mfa_level":
                    {
                        MfaLevel = AuditLogChange<MfaLevel>.SingleConvert(change, client.Serializer);
                        break;
                    }

                    case "verification_level":
                    {
                        VerificationLevel = AuditLogChange<VerificationLevel>.SingleConvert(change, client.Serializer);
                        break;
                    }

                    case "explicit_content_filter":
                    {
                        ContentFilterLevel = AuditLogChange<ContentFilterLevel>.SingleConvert(change, client.Serializer);
                        break;
                    }

                    case "default_message_notifications":
                    {
                        DefaultNotificationLevel = AuditLogChange<DefaultNotificationLevel>.SingleConvert(change, client.Serializer);
                        break;
                    }

                    case "vanity_url_code":
                    {
                        VanityUrlCode = AuditLogChange<string>.SingleConvert(change, client.Serializer);
                        break;
                    }

                    case "widget_enabled":
                    {
                        IsWidgetEnabled = AuditLogChange<bool>.SingleConvert(change, client.Serializer);
                        break;
                    }

                    case "widget_channel_id":
                    {
                        WidgetChannelId = AuditLogChange<Snowflake?>.DoubleConvert<ulong?>(change, client.Serializer, x => x);
                        break;
                    }

                    case "system_channel_id":
                    {
                        SystemChannelId = AuditLogChange<Snowflake?>.DoubleConvert<ulong?>(change, client.Serializer, x => x);
                        break;
                    }

                    default:
                    {
                        client.Log(LogMessageSeverity.Error, $"Unknown change key for {nameof(GuildChanges)}: '{change.Key}'.");
                        break;
                    }
                }
            }
        }
    }
}
