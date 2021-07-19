using System;
using Disqord.Models;

namespace Disqord.AuditLogs
{
    public class TransientAuditLog : TransientEntity<AuditLogEntryJsonModel>, IAuditLog
    {
        /// <inheritdoc/>
        public Snowflake Id => Model.Id;

        /// <inheritdoc/>
        public Snowflake GuildId { get; }

        /// <summary>
        ///     Gets the ID of the entity this audit log is targeting.
        /// </summary>
        public Snowflake? TargetId => Model.TargetId;

        /// <summary>
        ///     Gets the ID of the user this audit log was actioned by.
        /// </summary>
        public Snowflake? ActorId => Model.UserId;

        /// <summary>
        ///     Gets the optional user this audit log was actioned by.
        /// </summary>
        public IUser Actor
        {
            get
            {
                if (_actor == null)
                {
                    var actor = Array.Find(_auditLogJsonModel.Users, x => x.Id == ActorId);
                    if (actor != null)
                        _actor = new TransientUser(Client, actor);
                }

                return _actor;
            }
        }
        private IUser _actor;

        /// <summary>
        ///     Gets the reason of this audit log.
        /// </summary>
        public string Reason => Model.Reason.GetValueOrDefault();

        private readonly AuditLogJsonModel _auditLogJsonModel;

        public TransientAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
            : base(client, model)
        {
            _auditLogJsonModel = auditLogJsonModel;
            GuildId = guildId;
        }

        internal static IAuditLog Create(IClient client, Snowflake guildId, AuditLogJsonModel log, AuditLogEntryJsonModel entry) => entry.ActionType switch
        {
            // Guild
            AuditLogActionType.GuildUpdated => new TransientGuildUpdatedAuditLog(client, guildId, log, entry),

            // Channel
            AuditLogActionType.ChannelCreated => new TransientChannelCreatedAuditLog(client, guildId, log, entry),
            AuditLogActionType.ChannelUpdated => new TransientChannelUpdatedAuditLog(client, guildId, log, entry),
            AuditLogActionType.ChannelDeleted => new TransientChannelDeletedAuditLog(client, guildId, log, entry),

            // Overwrite
            AuditLogActionType.OverwriteCreated => new TransientOverwriteCreatedAuditLog(client, guildId, log, entry),
            AuditLogActionType.OverwriteUpdated => new TransientOverwriteUpdatedAuditLog(client, guildId, log, entry),
            AuditLogActionType.OverwriteDeleted => new TransientOverwriteDeletedAuditLog(client, guildId, log, entry),

            // Member
            AuditLogActionType.MemberKicked => new TransientMemberKickedAuditLog(client, guildId, log, entry),
            AuditLogActionType.MembersPruned => new TransientMembersPrunedAuditLog(client, guildId, log, entry),
            AuditLogActionType.MemberBanned => new TransientMemberBannedAuditLog(client, guildId, log, entry),
            AuditLogActionType.MemberUnbanned => new TransientMemberUnbannedAuditLog(client, guildId, log, entry),
            AuditLogActionType.MemberUpdated => new TransientMemberUpdatedAuditLog(client, guildId, log, entry),
            AuditLogActionType.MemberRolesUpdated => new TransientMemberRolesUpdatedAuditLog(client, guildId, log, entry),
            AuditLogActionType.MembersMoved => new TransientMembersMovedAuditLog(client, guildId, log, entry),
            AuditLogActionType.MembersDisconnected => new TransientMembersDisconnectedAuditLog(client, guildId, log, entry),
            AuditLogActionType.BotAdded => new TransientBotAddedAuditLog(client, guildId, log, entry),

            // Role
            AuditLogActionType.RoleCreated => new TransientRoleCreatedAuditLog(client, guildId, log, entry),
            AuditLogActionType.RoleUpdated => new TransientRoleUpdatedAuditLog(client, guildId, log, entry),
            AuditLogActionType.RoleDeleted => new TransientRoleDeletedAuditLog(client, guildId, log, entry),

            // Invite
            AuditLogActionType.InviteCreated => new TransientInviteCreatedAuditLog(client, guildId, log, entry),
            AuditLogActionType.InviteUpdated => new TransientInviteUpdatedAuditLog(client, guildId, log, entry),
            AuditLogActionType.InviteDeleted => new TransientInviteDeletedAuditLog(client, guildId, log, entry),

            // Webhook
            AuditLogActionType.WebhookCreated => new TransientWebhookCreatedAuditLog(client, guildId, log, entry),
            AuditLogActionType.WebhookUpdated => new TransientWebhookUpdatedAuditLog(client, guildId, log, entry),
            AuditLogActionType.WebhookDeleted => new TransientWebhookDeletedAuditLog(client, guildId, log, entry),

            // Emoji
            AuditLogActionType.EmojiCreated => new TransientEmojiCreatedAuditLog(client, guildId, log, entry),
            AuditLogActionType.EmojiUpdated => new TransientEmojiUpdatedAuditLog(client, guildId, log, entry),
            AuditLogActionType.EmojiDeleted => new TransientEmojiDeletedAuditLog(client, guildId, log, entry),

            // Message
            AuditLogActionType.MessagesDeleted => new TransientMessagesDeletedAuditLog(client, guildId, log, entry),
            AuditLogActionType.MessagesBulkDeleted => new TransientMessagesBulkDeletedAuditLog(client, guildId, log, entry),
            AuditLogActionType.MessagePinned => new TransientMessagePinnedAuditLog(client, guildId, log, entry),
            AuditLogActionType.MessageUnpinned => new TransientMessageUnpinnedAuditLog(client, guildId, log, entry),

            // Integration
            AuditLogActionType.IntegrationCreated => new TransientIntegrationCreatedAuditLog(client, guildId, log, entry),
            AuditLogActionType.IntegrationUpdated => new TransientIntegrationUpdatedAuditLog(client, guildId, log, entry),
            AuditLogActionType.IntegrationDeleted => new TransientIntegrationDeletedAuditLog(client, guildId, log, entry),

            // Stage
            AuditLogActionType.StageCreated => new TransientStageCreatedAuditLog(client, guildId, log, entry),
            AuditLogActionType.StageUpdated => new TransientStageUpdatedAuditLog(client, guildId, log, entry),
            AuditLogActionType.StageDeleted => new TransientStageDeletedAuditLog(client, guildId, log, entry),

            // Sticker
            AuditLogActionType.StickerCreated => new TransientStickerCreatedAuditLog(client, guildId, log, entry),
            AuditLogActionType.StickerUpdated => new TransientStickerUpdatedAuditLog(client, guildId, log, entry),
            AuditLogActionType.StickerDeleted => new TransientStickerDeletedAuditLog(client, guildId, log, entry),

            _ => new TransientUnknownAuditLog(client, guildId, log, entry),
        };
    }
}
