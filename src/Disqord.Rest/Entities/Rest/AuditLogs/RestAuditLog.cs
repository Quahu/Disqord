using System;
using Disqord.Models;

namespace Disqord.Rest.AuditLogs
{
    public abstract class RestAuditLog : RestSnowflakeEntity
    {
        public Snowflake ResponsibleUserId { get; }

        public RestDownloadable<RestUser> ResponsibleUser { get; }

        public string Reason { get; }

        internal RestAuditLog(RestDiscordClient client, AuditLogModel log, AuditLogEntryModel entry) : base(client, entry.Id)
        {
            ResponsibleUserId = entry.UserId;
            ResponsibleUser = new RestDownloadable<RestUser>(options => client.GetUserAsync(ResponsibleUserId, options));
            var userModel = Array.Find(log.Users, x => x.Id == entry.UserId);
            if (userModel != null)
                ResponsibleUser.SetValue(new RestUser(client, userModel));

            Reason = entry.Reason;
        }

        internal static RestAuditLog Create(RestDiscordClient client, AuditLogModel log, AuditLogEntryModel entry)
        {
            switch (entry.ActionType)
            {
                case AuditLogAction.GuildUpdated:
                    return new RestGuildUpdatedAuditLog(client, log, entry);

                case AuditLogAction.ChannelCreated:
                    break;

                case AuditLogAction.ChannelUpdated:
                    break;

                case AuditLogAction.ChannelDeleted:
                    break;

                case AuditLogAction.OverwriteCreated:
                    break;

                case AuditLogAction.OverwriteUpdated:
                    break;

                case AuditLogAction.OverwriteDeleted:
                    break;

                case AuditLogAction.MemberKicked:
                    break;

                case AuditLogAction.MembersPruned:
                    break;

                case AuditLogAction.MemberBanned:
                    break;

                case AuditLogAction.MemberUnbanned:
                    break;

                case AuditLogAction.MemberUpdated:
                    break;

                case AuditLogAction.MemberRolesUpdated:
                    break;

                case AuditLogAction.RoleCreated:
                    break;

                case AuditLogAction.RoleUpdated:
                    break;

                case AuditLogAction.RoleDeleted:
                    break;

                case AuditLogAction.InviteCreated:
                    break;

                case AuditLogAction.InviteUpdated:
                    break;

                case AuditLogAction.InviteDeleted:
                    break;

                case AuditLogAction.WebhookCreated:
                    break;

                case AuditLogAction.WebhookUpdated:
                    break;

                case AuditLogAction.WebhookDeleted:
                    break;

                case AuditLogAction.GuildEmojiCreated:
                    break;

                case AuditLogAction.GuildEmojiUpdated:
                    break;

                case AuditLogAction.GuildEmojiDeleted:
                    break;

                case AuditLogAction.MessagesDeleted:
                    break;

                default:
                    return new RestUnknownAuditLog(client, log, entry);
            }

            return new RestUnknownAuditLog(client, log, entry);
        }
    }
}