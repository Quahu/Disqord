using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Disqord.Rest.Pagination;

namespace Disqord.Rest
{
    public static partial class RestEntityExtensions
    {
        public static Task LeaveAsync(this IGuild guild, IRestRequestOptions options = null)
        {
            var client = guild.GetRestClient();
            return client.LeaveGuildAsync(guild.Id, options);
        }

        public static Task<IGuild> ModifyAsync(this IGuild guild, Action<ModifyGuildActionProperties> action, IRestRequestOptions options = null)
        {
            var client = guild.GetRestClient();
            return client.ModifyGuildAsync(guild.Id, action, options);
        }

        public static Task DeleteAsync(this IGuild guild, IRestRequestOptions options = null)
        {
            var client = guild.GetRestClient();
            return client.DeleteGuildAsync(guild.Id, options);
        }

        public static Task<IReadOnlyList<IGuildChannel>> FetchChannelsAsync(this IGuild guild, IRestRequestOptions options = null)
        {
            var client = guild.GetRestClient();
            return client.FetchChannelsAsync(guild.Id, options);
        }

        public static Task<ITextChannel> CreateTextChannelAsync(this IGuild guild, string name, Action<CreateTextChannelActionProperties> action = null, IRestRequestOptions options = null)
        {
            var client = guild.GetRestClient();
            return client.CreateTextChannelAsync(guild.Id, name, action, options);
        }

        public static Task<IVoiceChannel> CreateVoiceChannelAsync(this IGuild guild, string name, Action<CreateVoiceChannelActionProperties> action = null, IRestRequestOptions options = null)
        {
            var client = guild.GetRestClient();
            return client.CreateVoiceChannelAsync(guild.Id, name, action, options);
        }

        public static Task<ICategoryChannel> CreateCategoryChannelAsync(this IGuild guild, string name, Action<CreateCategoryChannelActionProperties> action = null, IRestRequestOptions options = null)
        {
            var client = guild.GetRestClient();
            return client.CreateCategoryChannelAsync(guild.Id, name, action, options);
        }

        public static Task ReorderChannelsAsync(this IGuild guild, IReadOnlyDictionary<Snowflake, int> positions, IRestRequestOptions options = null)
        {
            var client = guild.GetRestClient();
            return client.ReorderChannelsAsync(guild.Id, positions, options);
        }

        public static Task<IMember> FetchMemberAsync(this IGuild guild, Snowflake memberId, IRestRequestOptions options = null)
        {
            var client = guild.GetRestClient();
            return client.FetchMemberAsync(guild.Id, memberId, options);
        }

        public static IPagedEnumerable<IMember> EnumerateMembers(this IGuild guild, int limit, Snowflake? startFromId = null, IRestRequestOptions options = null)
        {
            var client = guild.GetRestClient();
            return client.EnumerateMembers(guild.Id, limit, startFromId, options);
        }

        public static Task<IReadOnlyList<IMember>> FetchMembersAsync(this IGuild guild, int limit = 1000, Snowflake? startFromId = null, IRestRequestOptions options = null)
        {
            var client = guild.GetRestClient();
            return client.FetchMembersAsync(guild.Id, limit, startFromId, options);
        }

        public static Task SetCurrentMemberNickAsync(this IGuild guild, string nick, IRestRequestOptions options = null)
        {
            var client = guild.GetRestClient();
            return client.SetCurrentMemberNickAsync(guild.Id, nick, options);
        }

        public static Task<IMember> ModifyMemberAsync(this IGuild guild, Snowflake memberId, Action<ModifyMemberActionProperties> action, IRestRequestOptions options = null)
        {
            var client = guild.GetRestClient();
            return client.ModifyMemberAsync(guild.Id, memberId, action, options);
        }

        public static Task GrantRoleAsync(this IGuild guild, Snowflake memberId, Snowflake roleId, IRestRequestOptions options = null)
        {
            var client = guild.GetRestClient();
            return client.GrantRoleAsync(guild.Id, memberId, roleId, options);
        }

        public static Task RevokeRoleAsync(this IGuild guild, Snowflake memberId, Snowflake roleId, IRestRequestOptions options = null)
        {
            var client = guild.GetRestClient();
            return client.RevokeRoleAsync(guild.Id, memberId, roleId, options);
        }

        public static Task KickMemberAsync(this IGuild guild, Snowflake memberId, IRestRequestOptions options = null)
        {
            var client = guild.GetRestClient();
            return client.KickMemberAsync(guild.Id, memberId, options);
        }

        public static Task<IReadOnlyList<IBan>> FetchBansAsync(this IGuild guild, IRestRequestOptions options = null)
        {
            var client = guild.GetRestClient();
            return client.FetchBansAsync(guild.Id, options);
        }

        public static Task<IBan> FetchBanAsync(this IGuild guild, Snowflake userId, IRestRequestOptions options = null)
        {
            var client = guild.GetRestClient();
            return client.FetchBanAsync(guild.Id, userId, options);
        }

        public static Task CreateBanAsync(this IGuild guild, Snowflake userId, string reason = null, int? deleteMessageDays = null, IRestRequestOptions options = null)
        {
            var client = guild.GetRestClient();
            return client.CreateBanAsync(guild.Id, userId, reason, deleteMessageDays, options);
        }

        public static Task DeleteBanAsync(this IGuild guild, Snowflake userId, IRestRequestOptions options = null)
        {
            var client = guild.GetRestClient();
            return client.DeleteBanAsync(guild.Id, userId, options);
        }

        public static Task<IReadOnlyList<IRole>> FetchRolesAsync(this IGuild guild, IRestRequestOptions options = null)
        {
            var client = guild.GetRestClient();
            return client.FetchRolesAsync(guild.Id, options);
        }

        public static Task<IRole> CreateRoleAsync(this IGuild guild, Action<CreateRoleActionProperties> action = null, IRestRequestOptions options = null)
        {
            var client = guild.GetRestClient();
            return client.CreateRoleAsync(guild.Id, action, options);
        }

        public static Task<IReadOnlyList<IRole>> ReorderRolesAsync(this IGuild guild, IReadOnlyDictionary<Snowflake, int> positions, IRestRequestOptions options = null)
        {
            var client = guild.GetRestClient();
            return client.ReorderRolesAsync(guild.Id, positions, options);
        }

        public static Task<IRole> ModifyRoleAsync(this IGuild guild, Snowflake roleId, Action<ModifyRoleActionProperties> action, IRestRequestOptions options = null)
        {
            var client = guild.GetRestClient();
            return client.ModifyRoleAsync(guild.Id, roleId, action, options);
        }

        public static Task DeleteRoleAsync(this IGuild guild, Snowflake roleId, IRestRequestOptions options = null)
        {
            var client = guild.GetRestClient();
            return client.DeleteRoleAsync(guild.Id, roleId, options);
        }

        public static Task<int> FetchPruneGuildCountAsync(this IGuild guild, int days, IEnumerable<Snowflake> roleIds = null, IRestRequestOptions options = null)
        {
            var client = guild.GetRestClient();
            return client.FetchPruneGuildCountAsync(guild.Id, days, roleIds, options);
        }

        public static Task<int?> PruneGuildAsync(this IGuild guild, int days, bool computePruneCount = true, IEnumerable<Snowflake> roleIds = null, IRestRequestOptions options = null)
        {
            var client = guild.GetRestClient();
            return client.PruneGuildAsync(guild.Id, days, computePruneCount, roleIds, options);
        }

        // TODO: fetch voice regions

        public static Task<IReadOnlyList<IInvite>> FetchInvitesAsync(this IGuild guild, IRestRequestOptions options = null)
        {
            var client = guild.GetRestClient();
            return client.FetchGuildInvitesAsync(guild.Id, options);
        }

        // TODO: fetch widget

        // TODO: modify widget

        // TODO: fetch vanity invite

        // TODO: fetch preview

        /*
         * Emoji
         */
        public static Task<IReadOnlyList<IGuildEmoji>> FetchEmojisAsync(this IGuild guild, IRestRequestOptions options = null)
        {
            var client = guild.GetRestClient();
            return client.FetchGuildEmojisAsync(guild.Id, options);
        }

        public static Task<IGuildEmoji> FetchEmojiAsync(this IGuild guild, Snowflake emojiId, IRestRequestOptions options = null)
        {
            var client = guild.GetRestClient();
            return client.FetchGuildEmojiAsync(guild.Id, emojiId, options);
        }

        public static Task<IGuildEmoji> CreateEmojiAsync(this IGuild guild, string name, Stream image, Action<CreateGuildEmojiActionProperties> action = null, IRestRequestOptions options = null)
        {
            var client = guild.GetRestClient();
            return client.CreateGuildEmojiAsync(guild.Id, name, image, action, options);
        }

        public static Task<IGuildEmoji> ModifyEmojiAsync(this IGuild guild, Snowflake emojiId, Action<ModifyGuildEmojiActionProperties> action, IRestRequestOptions options = null)
        {
            var client = guild.GetRestClient();
            return client.ModifyGuildEmojiAsync(guild.Id, emojiId, action, options);
        }

        public static Task DeleteEmojiAsync(this IGuild guild, Snowflake emojiId, IRestRequestOptions options = null)
        {
            var client = guild.GetRestClient();
            return client.DeleteGuildEmojiAsync(guild.Id, emojiId, options);
        }
    }
}
