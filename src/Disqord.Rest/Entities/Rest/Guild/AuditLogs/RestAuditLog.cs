using System;
using Disqord.Models;

namespace Disqord.Rest.AuditLogs
{
    public abstract class RestAuditLog : RestSnowflakeEntity
    {
        public Snowflake? TargetId { get; }

        public Snowflake ResponsibleUserId { get; }

        public RestFetchable<RestUser> ResponsibleUser { get; }

        public string Reason { get; }

        internal RestAuditLog(RestDiscordClient client, AuditLogModel log, AuditLogEntryModel entry) : base(client, entry.Id)
        {
            TargetId = entry.TargetId;
            ResponsibleUserId = entry.UserId;
            ResponsibleUser = RestFetchable.Create(this, (@this, options) =>
                @this.Client.GetUserAsync(@this.ResponsibleUserId, options));
            var userModel = Array.Find(log.Users, x => x.Id == entry.UserId);
            if (userModel != null)
                ResponsibleUser.Value = new RestUser(client, userModel);

            Reason = entry.Reason;
        }

        internal static RestAuditLog Create(RestDiscordClient client, AuditLogModel log, AuditLogEntryModel entry) => entry.ActionType switch
        {
            // Guild
            AuditLogType.GuildUpdated => new RestGuildUpdatedAuditLog(client, log, entry),

            // Channel
            AuditLogType.ChannelCreated => new RestChannelCreatedAuditLog(client, log, entry),
            AuditLogType.ChannelUpdated => new RestChannelUpdatedAuditLog(client, log, entry),
            AuditLogType.ChannelDeleted => new RestChannelDeletedAuditLog(client, log, entry),

            // Overwrite
            AuditLogType.OverwriteCreated => new RestOverwriteCreatedAuditLog(client, log, entry),
            AuditLogType.OverwriteUpdated => new RestOverwriteUpdatedAuditLog(client, log, entry),
            AuditLogType.OverwriteDeleted => new RestOverwriteDeletedAuditLog(client, log, entry),

            // Member
            AuditLogType.MemberKicked => new RestMemberKickedAuditLog(client, log, entry),
            AuditLogType.MembersPruned => new RestMembersPrunedAuditLog(client, log, entry),
            AuditLogType.MemberBanned => new RestMemberBannedAuditLog(client, log, entry),
            AuditLogType.MemberUnbanned => new RestMemberUnbannedAuditLog(client, log, entry),
            AuditLogType.MemberUpdated => new RestMemberUpdatedAuditLog(client, log, entry),
            AuditLogType.MemberRolesUpdated => new RestMemberRolesUpdatedAuditLog(client, log, entry),
            AuditLogType.MembersMoved => new RestMembersMovedAuditLog(client, log, entry),
            AuditLogType.MembersDisconnected => new RestMembersDisconnectedAuditLog(client, log, entry),
            AuditLogType.BotAdded => new RestBotAddedAuditLog(client, log, entry),

            // Role
            AuditLogType.RoleCreated => new RestRoleCreatedAuditLog(client, log, entry),
            AuditLogType.RoleUpdated => new RestRoleUpdatedAuditLog(client, log, entry),
            AuditLogType.RoleDeleted => new RestRoleDeletedAuditLog(client, log, entry),

            // TODO: Invite
            AuditLogType.InviteCreated => new RestInviteCreatedAuditLog(client, log, entry),
            AuditLogType.InviteUpdated => new RestInviteUpdatedAuditLog(client, log, entry),
            AuditLogType.InviteDeleted => new RestInviteDeletedAuditLog(client, log, entry),

            // Webhook
            AuditLogType.WebhookCreated => new RestWebhookCreatedAuditLog(client, log, entry),
            AuditLogType.WebhookUpdated => new RestWebhookUpdatedAuditLog(client, log, entry),
            AuditLogType.WebhookDeleted => new RestWebhookDeletedAuditLog(client, log, entry),

            // Emoji
            AuditLogType.EmojiCreated => new RestEmojiCreatedAuditLog(client, log, entry),
            AuditLogType.EmojiUpdated => new RestEmojiUpdatedAuditLog(client, log, entry),
            AuditLogType.EmojiDeleted => new RestEmojiDeletedAuditLog(client, log, entry),

            // Message
            AuditLogType.MessagesDeleted => new RestMessagesDeletedAuditLog(client, log, entry),
            AuditLogType.MessagesBulkDeleted => new RestMessagesBulkDeletedAuditLog(client, log, entry),
            AuditLogType.MessagePinned => new RestMessagePinnedAuditLog(client, log, entry),
            AuditLogType.MessageUnpinned => new RestMessageUnpinnedAuditLog(client, log, entry),

            // TODO: Integration
            AuditLogType.IntegrationCreated => new RestIntegrationCreatedAuditLog(client, log, entry),
            AuditLogType.IntegrationUpdated => new RestIntegrationUpdatedAuditLog(client, log, entry),
            AuditLogType.IntegrationDeleted => new RestIntegrationDeletedAuditLog(client, log, entry),

            _ => new RestUnknownAuditLog(client, log, entry),
        };
    }
}