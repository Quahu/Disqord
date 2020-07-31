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
                        Name = AuditLogChange<string>.Convert(change);
                        break;
                    }

                    case "icon_hash":
                    {
                        IconHash = AuditLogChange<string>.Convert(change);
                        break;
                    }

                    case "splash_hash":
                    {
                        SplashHash = AuditLogChange<string>.Convert(change);
                        break;
                    }

                    case "owner_id":
                    {
                        OwnerId = AuditLogChange<Snowflake>.Convert(change);
                        Owner = new AuditLogChange<SnowflakeOptional<RestUser>>(
                            new SnowflakeOptional<RestUser>(new RestUser(client, Array.Find(log.Users, x => x.Id == OwnerId.OldValue.Value)), OwnerId.OldValue.Value),
                            new SnowflakeOptional<RestUser>(new RestUser(client, Array.Find(log.Users, x => x.Id == OwnerId.NewValue.Value)), OwnerId.NewValue.Value));
                        break;
                    }

                    case "region":
                    {
                        VoiceRegionId = AuditLogChange<string>.Convert(change);
                        break;
                    }

                    case "afk_channel_id":
                    {
                        AfkChannelId = AuditLogChange<Snowflake?>.Convert(change);
                        break;
                    }

                    case "afk_timeout":
                    {
                        AfkTimeout = AuditLogChange<int>.Convert(change);
                        break;
                    }

                    case "mfa_level":
                    {
                        MfaLevel = AuditLogChange<MfaLevel>.Convert(change);
                        break;
                    }

                    case "verification_level":
                    {
                        VerificationLevel = AuditLogChange<VerificationLevel>.Convert(change);
                        break;
                    }

                    case "explicit_content_filter":
                    {
                        ContentFilterLevel = AuditLogChange<ContentFilterLevel>.Convert(change);
                        break;
                    }

                    case "default_message_notifications":
                    {
                        DefaultNotificationLevel = AuditLogChange<DefaultNotificationLevel>.Convert(change);
                        break;
                    }

                    case "vanity_url_code":
                    {
                        VanityUrlCode = AuditLogChange<string>.Convert(change);
                        break;
                    }

                    case "widget_enabled":
                    {
                        IsWidgetEnabled = AuditLogChange<bool>.Convert(change);
                        break;
                    }

                    case "widget_channel_id":
                    {
                        WidgetChannelId = AuditLogChange<Snowflake?>.Convert(change);
                        break;
                    }

                    case "system_channel_id":
                    {
                        SystemChannelId = AuditLogChange<Snowflake?>.Convert(change);
                        break;
                    }

                    default:
                    {
                        client.Log(LogSeverity.Error, $"Unknown change key for {nameof(GuildChanges)}: '{change.Key}'.");
                        break;
                    }
                }
            }
        }
    }
}
