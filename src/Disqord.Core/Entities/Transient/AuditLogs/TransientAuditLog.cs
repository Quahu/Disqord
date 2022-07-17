using System;
using Disqord.Models;
using Qommon;

namespace Disqord.AuditLogs;

public class TransientAuditLog : TransientClientEntity<AuditLogEntryJsonModel>, IAuditLog
{
    /// <inheritdoc/>
    public Snowflake Id => Model.Id;

    /// <inheritdoc/>
    public Snowflake GuildId { get; }

    /// <inheritdoc/>
    public Snowflake? TargetId => Model.TargetId;

    /// <inheritdoc/>
    public Snowflake? ActorId => Model.UserId;

    /// <inheritdoc/>
    public IUser? Actor
    {
        get
        {
            if (_actor == null)
            {
                var userModel = Array.Find(AuditLogJsonModel.Users, userModel => userModel.Id == ActorId);
                if (userModel != null)
                    _actor = new TransientUser(Client, userModel);
            }

            return _actor;
        }
    }
    private IUser? _actor;

    /// <inheritdoc/>
    public string? Reason => Model.Reason.GetValueOrDefault();

    protected readonly AuditLogJsonModel AuditLogJsonModel;

    public TransientAuditLog(IClient client, Snowflake guildId, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
        : base(client, model)
    {
        AuditLogJsonModel = auditLogJsonModel;
        GuildId = guildId;
    }

    internal static IAuditLog Create(IClient client, Snowflake guildId, AuditLogJsonModel log, AuditLogEntryJsonModel entry)
    {
        return entry.ActionType switch
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

            // Guild Event
            AuditLogActionType.GuildEventCreated => new TransientGuildEventCreatedAuditLog(client, guildId, log, entry),
            AuditLogActionType.GuildEventUpdated => new TransientGuildEventUpdatedAuditLog(client, guildId, log, entry),
            AuditLogActionType.GuildEventDeleted => new TransientGuildEventDeletedAuditLog(client, guildId, log, entry),

            // Thread
            AuditLogActionType.ThreadCreate => new TransientThreadCreatedAuditLog(client, guildId, log, entry),
            AuditLogActionType.ThreadUpdate => new TransientThreadUpdatedAuditLog(client, guildId, log, entry),
            AuditLogActionType.ThreadDelete => new TransientThreadDeletedAuditLog(client, guildId, log, entry),

            // Application Command Permission
            AuditLogActionType.ApplicationCommandPermissionsUpdate => new TransientApplicationCommandPermissionsUpdatedAuditLog(client, guildId, log, entry),

            // AutoModeration
            AuditLogActionType.AutoModerationRuleCreated => new TransientAutoModerationRuleCreatedAuditLog(client, guildId, log, entry),
            AuditLogActionType.AutoModerationRuleUpdated => new TransientAutoModerationRuleUpdatedAuditLog(client, guildId, log, entry),
            AuditLogActionType.AutoModerationRuleDeleted => new TransientAutoModerationRuleDeletedAuditLog(client, guildId, log, entry),
            AuditLogActionType.AutoModerationMessageBlocked => new TransientAutoModerationMessageBlockedAuditLog(client, guildId, log, entry),

            _ => new TransientUnknownAuditLog(client, guildId, log, entry)
        };
    }
}
