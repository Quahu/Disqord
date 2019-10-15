using System;
using Disqord.Models;

namespace Disqord.Rest.AuditLogs
{
    public sealed class RestAuditLogGuildMetadata : RestAuditLogMetadata
    {
        public AuditLogValue<string> Name { get; }

        public AuditLogValue<string> IconHash { get; }

        public AuditLogValue<string> SplashHash { get; }

        public AuditLogValue<ulong> OwnerId { get; }

        public AuditLogValue<RestUser> Owner { get; }

        public AuditLogValue<string> VoiceRegionId { get; }

        public AuditLogValue<ulong?> AfkChannelId { get; }

        public AuditLogValue<int> AfkTimeout { get; }

        public AuditLogValue<MfaLevel> MfaLevel { get; }

        public AuditLogValue<VerificationLevel> VerificationLevel { get; }

        public AuditLogValue<ExplicitFilterLevel> ExplicitFilterLevel { get; }

        public AuditLogValue<DefaultNotificationLevel> DefaultNotificationLevel { get; }

        public AuditLogValue<string> VanityUrlCode { get; }

        public AuditLogValue<bool> IsWidgetEnabled { get; }

        public AuditLogValue<ulong?> WidgetChannelId { get; }

        public AuditLogValue<ulong?> SystemChannelId { get; }

        internal RestAuditLogGuildMetadata(RestDiscordClient client, AuditLogModel auditLogModel, AuditLogEntryModel model) : base(client)
        {
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

                    case "icon_hash":
                    {
                        IconHash = new AuditLogValue<string>(change);
                        break;
                    }

                    case "splash_hash":
                    {
                        SplashHash = new AuditLogValue<string>(change);
                        break;
                    }

                    case "owner_id":
                    {
                        OwnerId = new AuditLogValue<ulong>(change);
                        var ownerBeforeModel = Array.Find(auditLogModel.Users, x => x.Id == OwnerId.OldValue);
                        var ownerAfterModel = Array.Find(auditLogModel.Users, x => x.Id == OwnerId.NewValue);
                        Owner = new AuditLogValue<RestUser>(
                            ownerBeforeModel != null
                                ? new RestUser(client, ownerBeforeModel)
                                : Optional<RestUser>.Empty,
                            ownerAfterModel != null
                                ? new RestUser(client, ownerAfterModel)
                                : Optional<RestUser>.Empty);
                        break;
                    }

                    case "region":
                    {
                        VoiceRegionId = new AuditLogValue<string>(change);
                        break;
                    }

                    case "afk_channel_id":
                    {
                        AfkChannelId = new AuditLogValue<ulong?>(change);
                        break;
                    }

                    case "afk_timeout":
                    {
                        AfkTimeout = new AuditLogValue<int>(change);
                        break;
                    }

                    case "mfa_level":
                    {
                        MfaLevel = new AuditLogValue<MfaLevel>(change);
                        break;
                    }

                    case "verification_level":
                    {
                        VerificationLevel = new AuditLogValue<VerificationLevel>(change);
                        break;
                    }

                    case "explicit_content_filter":
                    {
                        ExplicitFilterLevel = new AuditLogValue<ExplicitFilterLevel>(change);
                        break;
                    }

                    case "default_message_notifications":
                    {
                        DefaultNotificationLevel = new AuditLogValue<DefaultNotificationLevel>(change);
                        break;
                    }

                    case "vanity_url_code":
                    {
                        VanityUrlCode = new AuditLogValue<string>(change);
                        break;
                    }

                    case "widget_enabled":
                    {
                        IsWidgetEnabled = new AuditLogValue<bool>(change);
                        break;
                    }

                    case "widget_channel_id":
                    {
                        WidgetChannelId = new AuditLogValue<ulong?>(change);
                        break;
                    }

                    case "system_channel_id":
                    {
                        SystemChannelId = new AuditLogValue<ulong?>(change);
                        break;
                    }
                }
            }
        }
    }
}
