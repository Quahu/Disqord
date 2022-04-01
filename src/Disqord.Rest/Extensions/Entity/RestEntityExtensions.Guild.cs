using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Disqord.AuditLogs;
using Disqord.Rest.Pagination;

namespace Disqord.Rest
{
    public static partial class RestEntityExtensions
    {
        public static IPagedEnumerable<IAuditLog> EnumerateAuditLogs(this IGuild guild,
            int limit, Snowflake? actorId = null, Snowflake? startFromId = null,
            IRestRequestOptions options = null)
        {
            return guild.EnumerateAuditLogs<IAuditLog>(limit, actorId, startFromId, options);
        }

        public static IPagedEnumerable<TAuditLog> EnumerateAuditLogs<TAuditLog>(this IGuild guild,
            int limit, Snowflake? actorId = null, Snowflake? startFromId = null,
            IRestRequestOptions options = null)
            where TAuditLog : class, IAuditLog
        {
            var client = guild.GetRestClient();
            return client.EnumerateAuditLogs<TAuditLog>(guild.Id, limit, actorId, startFromId, options);
        }

        public static Task<IReadOnlyList<IAuditLog>> FetchAuditLogsAsync(this IGuild guild,
            int limit = Discord.Limits.Rest.FetchAuditLogsPageSize, Snowflake? actorId = null, Snowflake? startFromId = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            return guild.FetchAuditLogsAsync<IAuditLog>(limit, actorId, startFromId, options, cancellationToken);
        }

        public static Task<IReadOnlyList<TAuditLog>> FetchAuditLogsAsync<TAuditLog>(this IGuild guild,
            int limit = Discord.Limits.Rest.FetchAuditLogsPageSize, Snowflake? actorId = null, Snowflake? startFromId = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
            where TAuditLog : class, IAuditLog
        {
            var client = guild.GetRestClient();
            return client.FetchAuditLogsAsync<TAuditLog>(guild.Id, limit, actorId, startFromId, options, cancellationToken);
        }

        public static Task LeaveAsync(this IGuild guild,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.LeaveGuildAsync(guild.Id, options, cancellationToken);
        }

        public static Task<IGuild> ModifyAsync(this IGuild guild,
            Action<ModifyGuildActionProperties> action,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.ModifyGuildAsync(guild.Id, action, options, cancellationToken);
        }

        public static Task DeleteAsync(this IGuild guild,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.DeleteGuildAsync(guild.Id, options, cancellationToken);
        }

        public static Task<IReadOnlyList<IGuildChannel>> FetchChannelsAsync(this IGuild guild,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.FetchChannelsAsync(guild.Id, options, cancellationToken);
        }

        public static Task<ITextChannel> CreateTextChannelAsync(this IGuild guild,
            string name, Action<CreateTextChannelActionProperties> action = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.CreateTextChannelAsync(guild.Id, name, action, options, cancellationToken);
        }

        public static Task<IVoiceChannel> CreateVoiceChannelAsync(this IGuild guild,
            string name, Action<CreateVoiceChannelActionProperties> action = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.CreateVoiceChannelAsync(guild.Id, name, action, options, cancellationToken);
        }

        public static Task<ICategoryChannel> CreateCategoryChannelAsync(this IGuild guild,
            string name, Action<CreateCategoryChannelActionProperties> action = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.CreateCategoryChannelAsync(guild.Id, name, action, options, cancellationToken);
        }

        public static Task ReorderChannelsAsync(this IGuild guild,
            IReadOnlyDictionary<Snowflake, int> positions,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.ReorderChannelsAsync(guild.Id, positions, options, cancellationToken);
        }

        public static Task<IReadOnlyList<IThreadChannel>> FetchActiveThreadsAsync(this IGuild guild,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.FetchActiveThreadsAsync(guild.Id, options, cancellationToken);
        }

        public static Task<IMember> FetchMemberAsync(this IGuild guild,
            Snowflake memberId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.FetchMemberAsync(guild.Id, memberId, options, cancellationToken);
        }

        public static IPagedEnumerable<IMember> EnumerateMembers(this IGuild guild,
            int limit, Snowflake? startFromId = null,
            IRestRequestOptions options = null)
        {
            var client = guild.GetRestClient();
            return client.EnumerateMembers(guild.Id, limit, startFromId, options);
        }

        public static Task<IReadOnlyList<IMember>> FetchMembersAsync(this IGuild guild,
            int limit = Discord.Limits.Rest.FetchMembersPageSize, Snowflake? startFromId = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.FetchMembersAsync(guild.Id, limit, startFromId, options, cancellationToken);
        }

        public static Task<IReadOnlyList<IMember>> SearchMembersAsync(this IGuild guild,
            string query, int limit = Discord.Limits.Rest.FetchMembersPageSize,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.SearchMembersAsync(guild.Id, query, limit, options, cancellationToken);
        }

        [Obsolete("Use ModifyCurrentMemberAsync() instead.")]
        public static Task SetCurrentMemberNickAsync(this IGuild guild,
            string nick,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.SetCurrentMemberNickAsync(guild.Id, nick, options, cancellationToken);
        }

        public static Task<IMember> ModifyMemberAsync(this IGuild guild,
            Snowflake memberId, Action<ModifyMemberActionProperties> action,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.ModifyMemberAsync(guild.Id, memberId, action, options, cancellationToken);
        }

        public static Task<IMember> ModifyCurrentMemberAsync(this IGuild guild,
            Action<ModifyCurrentMemberActionProperties> action,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.ModifyCurrentMemberAsync(guild.Id, action, options, cancellationToken);
        }

        public static Task GrantRoleAsync(this IGuild guild,
            Snowflake memberId, Snowflake roleId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.GrantRoleAsync(guild.Id, memberId, roleId, options, cancellationToken);
        }

        public static Task RevokeRoleAsync(this IGuild guild,
            Snowflake memberId, Snowflake roleId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.RevokeRoleAsync(guild.Id, memberId, roleId, options, cancellationToken);
        }

        public static Task KickMemberAsync(this IGuild guild,
            Snowflake memberId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.KickMemberAsync(guild.Id, memberId, options, cancellationToken);
        }

        public static IPagedEnumerable<IBan> EnumerateBans(this IGuild guild,
            int limit, RetrievalDirection direction = RetrievalDirection.After, Snowflake? startFromId = null,
            IRestRequestOptions options = null)
        {
            var client = guild.GetRestClient();
            return client.EnumerateBans(guild.Id, limit, direction, startFromId, options);
        }

        public static Task<IReadOnlyList<IBan>> FetchBansAsync(this IGuild guild,
            int limit = Discord.Limits.Rest.FetchBansPageSize, RetrievalDirection direction = RetrievalDirection.After, Snowflake? startFromId = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.FetchBansAsync(guild.Id, limit, direction, startFromId, options, cancellationToken);
        }

        public static Task<IBan> FetchBanAsync(this IGuild guild,
            Snowflake userId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.FetchBanAsync(guild.Id, userId, options, cancellationToken);
        }

        public static Task CreateBanAsync(this IGuild guild,
            Snowflake userId, string reason = null, int? deleteMessageDays = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.CreateBanAsync(guild.Id, userId, reason, deleteMessageDays, options, cancellationToken);
        }

        public static Task DeleteBanAsync(this IGuild guild,
            Snowflake userId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.DeleteBanAsync(guild.Id, userId, options, cancellationToken);
        }

        public static Task<IReadOnlyList<IRole>> FetchRolesAsync(this IGuild guild,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.FetchRolesAsync(guild.Id, options, cancellationToken);
        }

        public static Task<IRole> CreateRoleAsync(this IGuild guild,
            Action<CreateRoleActionProperties> action = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.CreateRoleAsync(guild.Id, action, options, cancellationToken);
        }

        public static Task<IReadOnlyList<IRole>> ReorderRolesAsync(this IGuild guild,
            IReadOnlyDictionary<Snowflake, int> positions,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.ReorderRolesAsync(guild.Id, positions, options, cancellationToken);
        }

        public static Task<IRole> ModifyRoleAsync(this IGuild guild,
            Snowflake roleId, Action<ModifyRoleActionProperties> action,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.ModifyRoleAsync(guild.Id, roleId, action, options, cancellationToken);
        }

        public static Task DeleteRoleAsync(this IGuild guild,
            Snowflake roleId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.DeleteRoleAsync(guild.Id, roleId, options, cancellationToken);
        }

        public static Task<int> FetchPruneGuildCountAsync(this IGuild guild,
            int days, IEnumerable<Snowflake> roleIds = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.FetchPruneGuildCountAsync(guild.Id, days, roleIds, options, cancellationToken);
        }

        public static Task<int?> PruneGuildAsync(this IGuild guild,
            int days, bool computePruneCount = true, IEnumerable<Snowflake> roleIds = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.PruneGuildAsync(guild.Id, days, computePruneCount, roleIds, options, cancellationToken);
        }

        public static Task<IReadOnlyList<IGuildVoiceRegion>> FetchVoiceRegionsAsync(this IGuild guild,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.FetchGuildVoiceRegionsAsync(guild.Id, options, cancellationToken);
        }

        public static Task<IReadOnlyList<IInvite>> FetchInvitesAsync(this IGuild guild,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.FetchGuildInvitesAsync(guild.Id, options, cancellationToken);
        }

        public static Task<IReadOnlyList<IIntegration>> FetchIntegrationsAsync(this IGuild guild,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.FetchIntegrationsAsync(guild.Id, options, cancellationToken);
        }

        public static Task DeleteIntegrationAsync(this IGuild guild,
            Snowflake integrationId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.DeleteIntegrationAsync(guild.Id, integrationId, options, cancellationToken);
        }

        public static Task<IGuildWidget> FetchWidgetAsync(this IGuild guild,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.FetchWidgetAsync(guild.Id, options, cancellationToken);
        }

        public static Task<IGuildWidget> ModifyWidgetAsync(this IGuild guild,
            Action<ModifyWidgetActionProperties> action,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.ModifyWidgetAsync(guild.Id, action, options, cancellationToken);
        }

        public static Task<IVanityInvite> FetchVanityInviteAsync(this IGuild guild,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.FetchVanityInviteAsync(guild.Id, options, cancellationToken);
        }

        public static Task<IGuildPreview> FetchPreviewAsync(this IGuild guild,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.FetchPreviewAsync(guild.Id, options, cancellationToken);
        }

        /*
         * Emoji
         */
        public static Task<IReadOnlyList<IGuildEmoji>> FetchEmojisAsync(this IGuild guild,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.FetchGuildEmojisAsync(guild.Id, options, cancellationToken);
        }

        public static Task<IGuildEmoji> FetchEmojiAsync(this IGuild guild,
            Snowflake emojiId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.FetchGuildEmojiAsync(guild.Id, emojiId, options, cancellationToken);
        }

        public static Task<IGuildEmoji> CreateEmojiAsync(this IGuild guild,
            string name, Stream image, Action<CreateGuildEmojiActionProperties> action = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.CreateGuildEmojiAsync(guild.Id, name, image, action, options, cancellationToken);
        }

        public static Task<IGuildEmoji> ModifyEmojiAsync(this IGuild guild,
            Snowflake emojiId, Action<ModifyGuildEmojiActionProperties> action,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.ModifyGuildEmojiAsync(guild.Id, emojiId, action, options, cancellationToken);
        }

        public static Task DeleteEmojiAsync(this IGuild guild,
            Snowflake emojiId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.DeleteGuildEmojiAsync(guild.Id, emojiId, options, cancellationToken);
        }

        /*
         * Webhook
         */
        public static Task<IReadOnlyList<IWebhook>> FetchWebhooksAsync(this IGuild guild,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.FetchGuildWebhooksAsync(guild.Id, options, cancellationToken);
        }

        /*
         * Template
         */
        public static Task<IReadOnlyList<IGuildTemplate>> FetchTemplatesAsync(this IGuild guild,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.FetchTemplatesAsync(guild.Id, options, cancellationToken);
        }

        public static Task<IGuildTemplate> CreateTemplateAsync(this IGuild guild,
            string name, Action<CreateTemplateActionProperties> action = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.CreateTemplateAsync(guild.Id, name, action, options, cancellationToken);
        }

        public static Task<IGuildTemplate> SynchronizeTemplateAsync(this IGuild guild,
            string templateCode,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.SynchronizeTemplateAsync(guild.Id, templateCode, options, cancellationToken);
        }

        public static Task<IGuildTemplate> ModifyTemplateAsync(this IGuild guild,
            string templateCode, Action<ModifyTemplateActionProperties> action,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.ModifyTemplateAsync(guild.Id, templateCode, action, options, cancellationToken);
        }

        public static Task<IGuildTemplate> DeleteTemplateAsync(this IGuild guild,
            string templateCode,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.DeleteTemplateAsync(guild.Id, templateCode, options, cancellationToken);
        }

        /*
         * Sticker
         */
        public static Task<IReadOnlyList<IGuildSticker>> FetchStickersAsync(this IGuild guild,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.FetchGuildStickersAsync(guild.Id, options, cancellationToken);
        }

        public static Task<IGuildSticker> FetchStickerAsync(this IGuild guild,
            Snowflake stickerId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.FetchGuildStickerAsync(guild.Id, stickerId, options, cancellationToken);
        }

        public static Task<IGuildSticker> CreateStickerAsync(this IGuild guild,
            string name, string tags, Stream image, StickerFormatType imageType, string description = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.CreateGuildStickerAsync(guild.Id, name, tags, image, imageType, description, options, cancellationToken);
        }

        public static Task<IGuildSticker> ModifyStickerAsync(this IGuild guild,
            Snowflake stickerId, Action<ModifyGuildStickerActionProperties> action,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.ModifyGuildStickerAsync(guild.Id, stickerId, action, options, cancellationToken);
        }

        public static Task DeleteStickerAsync(this IGuild guild,
            Snowflake stickerId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.DeleteGuildStickerAsync(guild.Id, stickerId, options, cancellationToken);
        }

        /*
         * Voice State
        */
        public static Task ModifyCurrentMemberVoiceStateAsync(this IGuild guild,
            Snowflake channelId, Action<ModifyCurrentMemberVoiceStateActionProperties> action,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.ModifyCurrentMemberVoiceStateAsync(guild.Id, channelId, action, options, cancellationToken);
        }

        public static Task ModifyMemberVoiceStateAsync(this IGuild guild,
            Snowflake memberId, Snowflake channelId, Action<ModifyMemberVoiceStateActionProperties> action,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.ModifyMemberVoiceStateAsync(guild.Id, memberId, channelId, action, options, cancellationToken);
        }

        /*
         * Welcome Screen
         */
        public static Task<IGuildWelcomeScreen> FetchWelcomeScreenAsync(this IGuild guild,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.FetchGuildWelcomeScreenAsync(guild.Id, options, cancellationToken);
        }

        public static Task<IGuildWelcomeScreen> ModifyWelcomeScreenAsync(this IGuild guild,
            Action<ModifyWelcomeScreenActionProperties> action,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.ModifyGuildWelcomeScreenAsync(guild.Id, action, options, cancellationToken);
        }

        /*
         * Application Commands
         */
        public static Task<IReadOnlyList<IApplicationCommand>> FetchApplicationCommandsAsync(this IGuild guild,
            Snowflake applicationId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.FetchGuildApplicationCommandsAsync(applicationId, guild.Id, options, cancellationToken);
        }

        public static Task<IApplicationCommand> CreateApplicationCommandAsync(this IGuild guild,
            Snowflake applicationId, LocalApplicationCommand command,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.CreateGuildApplicationCommandAsync(applicationId, guild.Id, command, options, cancellationToken);
        }

        public static Task<IApplicationCommand> FetchApplicationCommandAsync(this IGuild guild,
            Snowflake applicationId, Snowflake commandId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.FetchGuildApplicationCommandAsync(applicationId, guild.Id, commandId, options, cancellationToken);
        }

        public static Task<IApplicationCommand> ModifyApplicationCommandAsync(this IGuild guild,
            Snowflake applicationId, Snowflake commandId, Action<ModifyApplicationCommandActionProperties> action,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.ModifyGuildApplicationCommandAsync(applicationId, guild.Id, commandId, action, options, cancellationToken);
        }

        public static Task DeleteApplicationCommandAsync(this IGuild guild,
            Snowflake applicationId, Snowflake commandId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.DeleteGuildApplicationCommandAsync(applicationId, guild.Id, commandId, options, cancellationToken);
        }

        /*
         * Guild Events
         */
        public static Task<IReadOnlyList<IGuildEvent>> FetchEventsAsync(this IGuild guild,
            bool? withSubscriberCount = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.FetchGuildEventsAsync(guild.Id, withSubscriberCount, options, cancellationToken);
        }

        public static Task<IGuildEvent> CreateEventAsync(this IGuild guild,
            string name, DateTimeOffset startsAt, GuildEventTargetType targetType,
            PrivacyLevel privacyLevel = PrivacyLevel.GuildOnly, Action<CreateGuildEventActionProperties> action = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.CreateGuildEventAsync(guild.Id, name, startsAt, targetType, privacyLevel, action, options, cancellationToken);
        }

        public static Task<IGuildEvent> FetchEventAsync(this IGuild guild,
            Snowflake eventId, bool? withUserCount = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.FetchGuildEventAsync(guild.Id, eventId, withUserCount, options, cancellationToken);
        }

        public static Task<IGuildEvent> ModifyEventAsync(this IGuild guild,
            Snowflake eventId, Action<ModifyGuildEventActionProperties> action,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.ModifyGuildEventAsync(guild.Id, eventId, action, options, cancellationToken);
        }

        public static Task DeleteEventAsync(this IGuild guild,
            Snowflake eventId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.DeleteGuildEventAsync(guild.Id, eventId, options, cancellationToken);
        }

        public static IPagedEnumerable<IUser> EnumerateEventUsers(this IGuild guild,
            Snowflake eventId, int limit, RetrievalDirection direction = RetrievalDirection.After,
            Snowflake? startFromId = null, bool? withMember = null,
            IRestRequestOptions options = null)
        {
            var client = guild.GetRestClient();
            return client.EnumerateGuildEventUsers(guild.Id, eventId, limit, direction, startFromId, withMember, options);
        }

        public static Task<IReadOnlyList<IUser>> FetchEventUsersAsync(this IGuild guild,
            Snowflake eventId, int limit = Discord.Limits.Rest.FetchGuildEventUsersPageSize,
            RetrievalDirection direction = RetrievalDirection.After, Snowflake? startFromId = null,
            bool? withMember = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = guild.GetRestClient();
            return client.FetchGuildEventUsersAsync(guild.Id, eventId, limit, direction, startFromId, withMember, options, cancellationToken);
        }
    }
}
