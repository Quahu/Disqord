using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Collections;
using Disqord.Rest.AuditLogs;

namespace Disqord.Rest
{
    public partial class RestDiscordClient : IRestDiscordClient
    {
        public RestRequestEnumerable<RestAuditLog> GetAuditLogsEnumerable(Snowflake guildId, int limit = 100, Snowflake? userId = null, Snowflake? startFromId = null, RestRequestOptions options = null)
            => GetAuditLogsEnumerable<RestAuditLog>(guildId, limit, userId, startFromId, options);

        public RestRequestEnumerable<T> GetAuditLogsEnumerable<T>(Snowflake guildId, int limit = 100, Snowflake? userId = null, Snowflake? startFromId = null, RestRequestOptions options = null) where T : RestAuditLog
            => new RestRequestEnumerable<T>(new RestAuditLogsRequestEnumerator<T>(this, guildId, limit, userId, startFromId, options));

        public Task<IReadOnlyList<RestAuditLog>> GetAuditLogsAsync(Snowflake guildId, int limit = 100, Snowflake? userId = null, Snowflake? startFromId = null, RestRequestOptions options = null)
            => GetAuditLogsAsync<RestAuditLog>(guildId, limit, userId, startFromId, options);

        public Task<IReadOnlyList<T>> GetAuditLogsAsync<T>(Snowflake guildId, int limit = 100, Snowflake? userId = null, Snowflake? startFromId = null, RestRequestOptions options = null) where T : RestAuditLog
        {
            if (limit == 0)
                return Task.FromResult(ReadOnlyList<T>.Empty);

            if (limit <= 100)
                return InternalGetAuditLogsAsync<T>(guildId, limit, userId, startFromId, options);

            var enumerable = GetAuditLogsEnumerable<T>(guildId, limit, userId, startFromId, options);
            return enumerable.FlattenAsync();
        }

        internal async Task<IReadOnlyList<T>> InternalGetAuditLogsAsync<T>(Snowflake guildId, int limit = 100, Snowflake? userId = null, Snowflake? startFromId = null, RestRequestOptions options = null) where T : RestAuditLog
        {
            var model = await ApiClient.GetGuildAuditLogAsync(guildId, limit, userId, GetAuditLogAction(typeof(T)), startFromId, options).ConfigureAwait(false);
            return model.AuditLogEntries.Select(x => RestAuditLog.Create(this, model, x)).OfType<T>().ToReadOnlyList();
        }

        private AuditLogType? GetAuditLogAction(Type type)
        {
            // Any
            if (type == typeof(RestAuditLog) || type == typeof(RestUnknownAuditLog))
                return null;


            // Guild
            if (type == typeof(RestGuildUpdatedAuditLog))
                return AuditLogType.GuildUpdated;


            // Channel
            if (type == typeof(RestChannelCreatedAuditLog))
                return AuditLogType.ChannelCreated;

            if (type == typeof(RestChannelUpdatedAuditLog))
                return AuditLogType.ChannelUpdated;

            if (type == typeof(RestChannelDeletedAuditLog))
                return AuditLogType.ChannelDeleted;


            // Overwrite
            if (type == typeof(RestOverwriteCreatedAuditLog))
                return AuditLogType.OverwriteCreated;

            if (type == typeof(RestOverwriteUpdatedAuditLog))
                return AuditLogType.OverwriteUpdated;

            if (type == typeof(RestOverwriteDeletedAuditLog))
                return AuditLogType.OverwriteDeleted;


            // Member
            if (type == typeof(RestMemberKickedAuditLog))
                return AuditLogType.MemberKicked;

            if (type == typeof(RestMembersPrunedAuditLog))
                return AuditLogType.MembersPruned;

            if (type == typeof(RestMemberBannedAuditLog))
                return AuditLogType.MemberBanned;

            if (type == typeof(RestMemberUnbannedAuditLog))
                return AuditLogType.MemberUnbanned;

            if (type == typeof(RestMemberUpdatedAuditLog))
                return AuditLogType.MemberUpdated;

            if (type == typeof(RestMemberRolesUpdatedAuditLog))
                return AuditLogType.MemberRolesUpdated;

            if (type == typeof(RestMembersMovedAuditLog))
                return AuditLogType.MembersMoved;

            if (type == typeof(RestMembersDisconnectedAuditLog))
                return AuditLogType.MembersDisconnected;

            if (type == typeof(RestBotAddedAuditLog))
                return AuditLogType.BotAdded;


            // Role
            if (type == typeof(RestRoleCreatedAuditLog))
                return AuditLogType.RoleCreated;

            if (type == typeof(RestRoleUpdatedAuditLog))
                return AuditLogType.RoleUpdated;

            if (type == typeof(RestRoleDeletedAuditLog))
                return AuditLogType.RoleDeleted;


            // Invite
            if (type == typeof(RestInviteCreatedAuditLog))
                return AuditLogType.InviteCreated;

            if (type == typeof(RestInviteUpdatedAuditLog))
                return AuditLogType.InviteUpdated;

            if (type == typeof(RestInviteDeletedAuditLog))
                return AuditLogType.InviteDeleted;


            // Webhook
            if (type == typeof(RestWebhookCreatedAuditLog))
                return AuditLogType.WebhookCreated;

            if (type == typeof(RestWebhookUpdatedAuditLog))
                return AuditLogType.WebhookUpdated;

            if (type == typeof(RestWebhookDeletedAuditLog))
                return AuditLogType.WebhookDeleted;


            // Emoji
            if (type == typeof(RestEmojiCreatedAuditLog))
                return AuditLogType.EmojiCreated;

            if (type == typeof(RestEmojiUpdatedAuditLog))
                return AuditLogType.EmojiUpdated;

            if (type == typeof(RestEmojiDeletedAuditLog))
                return AuditLogType.EmojiDeleted;


            // Message
            if (type == typeof(RestMessagesDeletedAuditLog))
                return AuditLogType.MessagesDeleted;

            if (type == typeof(RestMessagesBulkDeletedAuditLog))
                return AuditLogType.MessagesBulkDeleted;

            if (type == typeof(RestMessagePinnedAuditLog))
                return AuditLogType.MessagePinned;

            if (type == typeof(RestMessageUnpinnedAuditLog))
                return AuditLogType.MessageUnpinned;


            // Integration
            if (type == typeof(RestIntegrationCreatedAuditLog))
                return AuditLogType.IntegrationCreated;

            if (type == typeof(RestIntegrationUpdatedAuditLog))
                return AuditLogType.IntegrationUpdated;

            if (type == typeof(RestIntegrationDeletedAuditLog))
                return AuditLogType.IntegrationDeleted;

            throw new ArgumentOutOfRangeException(nameof(type));
        }
    }
}
