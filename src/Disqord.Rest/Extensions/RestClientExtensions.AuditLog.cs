using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disqord.AuditLogs;
using Disqord.Rest.Api;
using Disqord.Rest.Pagination;
using Qommon;
using Qommon.Collections;
using Qommon.Collections.ReadOnly;

namespace Disqord.Rest
{
    public static partial class RestClientExtensions
    {
        // TODO: AuditLogActionType overload?
        public static IPagedEnumerable<IAuditLog> EnumerateAuditLogs(this IRestClient client,
            Snowflake guildId, int limit, Snowflake? actorId = null, Snowflake? startFromId = null,
            IRestRequestOptions options = null)
            => client.EnumerateAuditLogs<IAuditLog>(guildId, limit, actorId, startFromId, options);

        public static IPagedEnumerable<TAuditLog> EnumerateAuditLogs<TAuditLog>(this IRestClient client,
            Snowflake guildId, int limit, Snowflake? actorId = null, Snowflake? startFromId = null,
            IRestRequestOptions options = null)
            where TAuditLog : class, IAuditLog
            => PagedEnumerable.Create(static (state, cancellationToken) =>
            {
                var (client, guildId, limit, actorId, startFromId, options) = state;
                return new FetchAuditLogsPagedEnumerator<TAuditLog>(client, guildId, limit, actorId, startFromId, options, cancellationToken);
            }, (client, guildId, limit, actorId, startFromId, options));

        public static Task<IReadOnlyList<IAuditLog>> FetchAuditLogsAsync(this IRestClient client,
            Snowflake guildId, int limit = Discord.Limits.Rest.FetchAuditLogsPageSize, Snowflake? actorId = null, Snowflake? startFromId = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
            => client.FetchAuditLogsAsync<IAuditLog>(guildId, limit, actorId, startFromId, options, cancellationToken);

        public static Task<IReadOnlyList<TAuditLog>> FetchAuditLogsAsync<TAuditLog>(this IRestClient client,
            Snowflake guildId, int limit = Discord.Limits.Rest.FetchAuditLogsPageSize, Snowflake? actorId = null, Snowflake? startFromId = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
            where TAuditLog : class, IAuditLog
        {
            if (limit == 0)
                return Task.FromResult(ReadOnlyList<TAuditLog>.Empty);

            if (limit <= 100)
                return client.InternalFetchAuditLogsAsync<TAuditLog>(guildId, limit, actorId, startFromId, options, cancellationToken);

            var enumerable = client.EnumerateAuditLogs<TAuditLog>(guildId, limit, actorId, startFromId, options);
            return enumerable.FlattenAsync(cancellationToken);
        }

        internal static async Task<IReadOnlyList<TAuditLog>> InternalFetchAuditLogsAsync<TAuditLog>(this IRestClient client,
            Snowflake guildId, int limit, Snowflake? actorId, Snowflake? startFromId,
            IRestRequestOptions options, CancellationToken cancellationToken)
            where TAuditLog : IAuditLog
        {
            var type = GetAuditLogActionType(typeof(TAuditLog));
            var model = await client.ApiClient.FetchAuditLogsAsync(guildId, limit, actorId, type, startFromId, options, cancellationToken).ConfigureAwait(false);
            var list = new List<TAuditLog>();
            foreach (var entry in model.AuditLogEntries)
            {
                if (type != null && entry.ActionType != type.Value)
                    continue;

                if (TransientAuditLog.Create(client, guildId, model, entry) is TAuditLog auditLog)
                    list.Add(auditLog);
            }

            return list.ReadOnly();
        }

        private static AuditLogActionType? GetAuditLogActionType(Type type)
        {
            // Any
            if (type == typeof(IAuditLog) || type == typeof(IUnknownAuditLog)
                || type == typeof(TransientAuditLog) || type == typeof(TransientUnknownAuditLog))
                return null;

            // Guild
            if (typeof(IGuildUpdatedAuditLog).IsAssignableFrom(type))
                return AuditLogActionType.GuildUpdated;

            // Channel
            if (typeof(IChannelCreatedAuditLog).IsAssignableFrom(type))
                return AuditLogActionType.ChannelCreated;

            if (typeof(IChannelUpdatedAuditLog).IsAssignableFrom(type))
                return AuditLogActionType.ChannelUpdated;

            if (typeof(IChannelDeletedAuditLog).IsAssignableFrom(type))
                return AuditLogActionType.ChannelDeleted;

            // Overwrite
            if (typeof(IOverwriteCreatedAuditLog).IsAssignableFrom(type))
                return AuditLogActionType.OverwriteCreated;

            if (typeof(IOverwriteUpdatedAuditLog).IsAssignableFrom(type))
                return AuditLogActionType.OverwriteUpdated;

            if (typeof(IOverwriteDeletedAuditLog).IsAssignableFrom(type))
                return AuditLogActionType.OverwriteDeleted;

            // Member
            if (typeof(IMemberKickedAuditLog).IsAssignableFrom(type))
                return AuditLogActionType.MemberKicked;

            if (typeof(IMembersPrunedAuditLog).IsAssignableFrom(type))
                return AuditLogActionType.MembersPruned;

            if (typeof(IMemberBannedAuditLog).IsAssignableFrom(type))
                return AuditLogActionType.MemberBanned;

            if (typeof(IMemberUnbannedAuditLog).IsAssignableFrom(type))
                return AuditLogActionType.MemberUnbanned;

            if (typeof(IMemberUpdatedAuditLog).IsAssignableFrom(type))
                return AuditLogActionType.MemberUpdated;

            if (typeof(IMemberRolesUpdatedAuditLog).IsAssignableFrom(type))
                return AuditLogActionType.MemberRolesUpdated;

            if (typeof(IMembersMovedAuditLog).IsAssignableFrom(type))
                return AuditLogActionType.MembersMoved;

            if (typeof(IMembersDisconnectedAuditLog).IsAssignableFrom(type))
                return AuditLogActionType.MembersDisconnected;

            if (typeof(IBotAddedAuditLog).IsAssignableFrom(type))
                return AuditLogActionType.BotAdded;

            // Role
            if (typeof(IRoleCreatedAuditLog).IsAssignableFrom(type))
                return AuditLogActionType.RoleCreated;

            if (typeof(IRoleUpdatedAuditLog).IsAssignableFrom(type))
                return AuditLogActionType.RoleUpdated;

            if (typeof(IRoleDeletedAuditLog).IsAssignableFrom(type))
                return AuditLogActionType.RoleDeleted;

            // Invite
            if (typeof(IInviteCreatedAuditLog).IsAssignableFrom(type))
                return AuditLogActionType.InviteCreated;

            if (typeof(IInviteUpdatedAuditLog).IsAssignableFrom(type))
                return AuditLogActionType.InviteUpdated;

            if (typeof(IInviteDeletedAuditLog).IsAssignableFrom(type))
                return AuditLogActionType.InviteDeleted;

            // Webhook
            if (typeof(IWebhookCreatedAuditLog).IsAssignableFrom(type))
                return AuditLogActionType.WebhookCreated;

            if (typeof(IWebhookUpdatedAuditLog).IsAssignableFrom(type))
                return AuditLogActionType.WebhookUpdated;

            if (typeof(IWebhookDeletedAuditLog).IsAssignableFrom(type))
                return AuditLogActionType.WebhookDeleted;

            // Emoji
            if (typeof(IEmojiCreatedAuditLog).IsAssignableFrom(type))
                return AuditLogActionType.EmojiCreated;

            if (typeof(IEmojiUpdatedAuditLog).IsAssignableFrom(type))
                return AuditLogActionType.EmojiUpdated;

            if (typeof(IEmojiDeletedAuditLog).IsAssignableFrom(type))
                return AuditLogActionType.EmojiDeleted;

            // Message
            if (typeof(IMessagesDeletedAuditLog).IsAssignableFrom(type))
                return AuditLogActionType.MessagesDeleted;

            if (typeof(IMessagesBulkDeletedAuditLog).IsAssignableFrom(type))
                return AuditLogActionType.MessagesBulkDeleted;

            if (typeof(IMessagePinnedAuditLog).IsAssignableFrom(type))
                return AuditLogActionType.MessagePinned;

            if (typeof(IMessageUnpinnedAuditLog).IsAssignableFrom(type))
                return AuditLogActionType.MessageUnpinned;

            // Integration
            if (typeof(IIntegrationCreatedAuditLog).IsAssignableFrom(type))
                return AuditLogActionType.IntegrationCreated;

            if (typeof(IIntegrationUpdatedAuditLog).IsAssignableFrom(type))
                return AuditLogActionType.IntegrationUpdated;

            if (typeof(IIntegrationDeletedAuditLog).IsAssignableFrom(type))
                return AuditLogActionType.IntegrationDeleted;

            // Thread
            if (typeof(IThreadCreatedAuditLog).IsAssignableFrom(type))
                return AuditLogActionType.ThreadCreate;

            if (typeof(IThreadUpdatedAuditLog).IsAssignableFrom(type))
                return AuditLogActionType.ThreadUpdate;

            if (typeof(IThreadDeletedAuditLog).IsAssignableFrom(type))
                return AuditLogActionType.ThreadDelete;

            // Stage
            if (typeof(IStageCreatedAuditLog).IsAssignableFrom(type))
                return AuditLogActionType.StageCreated;

            if (typeof(IStageUpdatedAuditLog).IsAssignableFrom(type))
                return AuditLogActionType.StageUpdated;

            if (typeof(IStageDeletedAuditLog).IsAssignableFrom(type))
                return AuditLogActionType.StageDeleted;

            // Sticker
            if (typeof(IStickerCreatedAuditLog).IsAssignableFrom(type))
                return AuditLogActionType.StickerCreated;

            if (typeof(IStickerUpdatedAuditLog).IsAssignableFrom(type))
                return AuditLogActionType.StickerUpdated;

            if (typeof(IStickerDeletedAuditLog).IsAssignableFrom(type))
                return AuditLogActionType.StickerDeleted;

            return Throw.ArgumentOutOfRangeException<AuditLogActionType?>(nameof(type));
        }
    }
}
