using System;
using Disqord.Models;

namespace Disqord.Rest.AuditLogs
{
    public abstract class RestAuditLog : RestSnowflakeEntity
    {
        public Snowflake? TargetId { get; }

        public Snowflake ResponsibleUserId { get; }

        public RestDownloadable<RestUser> ResponsibleUser { get; }

        public string Reason { get; }

        internal RestAuditLog(RestDiscordClient client, AuditLogModel log, AuditLogEntryModel entry) : base(client, entry.Id)
        {
            TargetId = entry.TargetId;
            ResponsibleUserId = entry.UserId;
            ResponsibleUser = new RestDownloadable<RestUser>(options => Client.GetUserAsync(ResponsibleUserId, options));
            var userModel = Array.Find(log.Users, x => x.Id == entry.UserId);
            if (userModel != null)
                ResponsibleUser.SetValue(new RestUser(client, userModel));

            Reason = entry.Reason;
        }

        internal static RestAuditLog Create(RestDiscordClient client, AuditLogModel log, AuditLogEntryModel entry)
        {
            switch (entry.ActionType)
            {
                // Guild
                case AuditLogType.GuildUpdated:
                    return new RestGuildUpdatedAuditLog(client, log, entry);


                // Channel
                case AuditLogType.ChannelCreated:
                    return new RestChannelCreatedAuditLog(client, log, entry);

                case AuditLogType.ChannelUpdated:
                    return new RestChannelUpdatedAuditLog(client, log, entry);

                case AuditLogType.ChannelDeleted:
                    return new RestChannelDeletedAuditLog(client, log, entry);


                // Overwrite
                case AuditLogType.OverwriteCreated:
                    return new RestOverwriteCreatedAuditLog(client, log, entry);

                case AuditLogType.OverwriteUpdated:
                    return new RestOverwriteUpdatedAuditLog(client, log, entry);

                case AuditLogType.OverwriteDeleted:
                    return new RestOverwriteDeletedAuditLog(client, log, entry);


                // Member
                case AuditLogType.MemberKicked:
                    return new RestMemberKickedAuditLog(client, log, entry);

                case AuditLogType.MembersPruned:
                    return new RestMembersPrunedAuditLog(client, log, entry);

                case AuditLogType.MemberBanned:
                    return new RestMemberBannedAuditLog(client, log, entry);

                case AuditLogType.MemberUnbanned:
                    return new RestMemberUnbannedAuditLog(client, log, entry);

                case AuditLogType.MemberUpdated:
                    return new RestMemberUpdatedAuditLog(client, log, entry);

                case AuditLogType.MemberRolesUpdated:
                    return new RestMemberRolesUpdatedAuditLog(client, log, entry);

                case AuditLogType.MembersMoved:
                    return new RestMembersMovedAuditLog(client, log, entry);

                case AuditLogType.MembersDisconnected:
                    return new RestMembersDisconnectedAuditLog(client, log, entry);

                case AuditLogType.BotAdded:
                    return new RestBotAddedAuditLog(client, log, entry);


                // Role
                case AuditLogType.RoleCreated:
                    return new RestRoleCreatedAuditLog(client, log, entry);

                case AuditLogType.RoleUpdated:
                    return new RestRoleUpdatedAuditLog(client, log, entry);

                case AuditLogType.RoleDeleted:
                    return new RestRoleDeletedAuditLog(client, log, entry);


                // Invite (TODO)
                case AuditLogType.InviteCreated:
                    return new RestInviteCreatedAuditLog(client, log, entry);

                case AuditLogType.InviteUpdated:
                    return new RestInviteUpdatedAuditLog(client, log, entry);

                case AuditLogType.InviteDeleted:
                    return new RestInviteDeletedAuditLog(client, log, entry);


                // Webhook
                case AuditLogType.WebhookCreated:
                    return new RestWebhookCreatedAuditLog(client, log, entry);

                case AuditLogType.WebhookUpdated:
                    return new RestWebhookUpdatedAuditLog(client, log, entry);

                case AuditLogType.WebhookDeleted:
                    return new RestWebhookDeletedAuditLog(client, log, entry);


                // Emoji
                case AuditLogType.EmojiCreated:
                    return new RestEmojiCreatedAuditLog(client, log, entry);

                case AuditLogType.EmojiUpdated:
                    return new RestEmojiUpdatedAuditLog(client, log, entry);

                case AuditLogType.EmojiDeleted:
                    return new RestEmojiDeletedAuditLog(client, log, entry);


                // Message
                case AuditLogType.MessagesDeleted:
                    return new RestMessagesDeletedAuditLog(client, log, entry);

                case AuditLogType.MessagesBulkDeleted:
                    return new RestMessagesBulkDeletedAuditLog(client, log, entry);

                case AuditLogType.MessagePinned:
                    return new RestMessagePinnedAuditLog(client, log, entry);

                case AuditLogType.MessageUnpinned:
                    return new RestMessageUnpinnedAuditLog(client, log, entry);


                // Integration (TODO)
                case AuditLogType.IntegrationCreated:
                    return new RestIntegrationCreatedAuditLog(client, log, entry);

                case AuditLogType.IntegrationUpdated:
                    return new RestIntegrationUpdatedAuditLog(client, log, entry);

                case AuditLogType.IntegrationDeleted:
                    return new RestIntegrationDeletedAuditLog(client, log, entry);
            }

            return new RestUnknownAuditLog(client, log, entry);
        }
    }
}