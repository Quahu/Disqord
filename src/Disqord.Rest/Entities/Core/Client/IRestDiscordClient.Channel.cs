﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord
{
    public partial interface IRestDiscordClient : IDisposable
    {
        Task<RestChannel> GetChannelAsync(Snowflake channelId, RestRequestOptions options = null);

        Task<RestGroupChannel> ModifyGroupChannelAsync(Snowflake channelId, Action<ModifyGroupChannelProperties> action, RestRequestOptions options = null);

        Task<RestTextChannel> ModifyTextChannelAsync(Snowflake channelId, Action<ModifyTextChannelProperties> action, RestRequestOptions options = null);

        Task<RestVoiceChannel> ModifyVoiceChannelAsync(Snowflake channelId, Action<ModifyVoiceChannelProperties> action, RestRequestOptions options = null);

        Task<RestCategoryChannel> ModifyCategoryChannelAsync(Snowflake channelId, Action<ModifyCategoryChannelProperties> action, RestRequestOptions options = null);

        Task DeleteOrCloseChannelAsync(Snowflake channelId, RestRequestOptions options = null);

        RestRequestEnumerator<RestMessage> GetMessagesEnumerator(Snowflake channelId, int limit, RetrievalDirection? direction = null, Snowflake? startFromId = null);

        Task<IReadOnlyList<RestMessage>> GetMessagesAsync(Snowflake channelId, int limit = 100, RetrievalDirection? direction = null, Snowflake? startFromId = null, RestRequestOptions options = null);

        Task<RestMessage> GetMessageAsync(Snowflake channelId, Snowflake messageId, RestRequestOptions options = null);

        Task<RestUserMessage> SendMessageAsync(Snowflake channelId, string content = null, bool textToSpeech = false, Embed embed = null, RestRequestOptions options = null);

        Task<RestUserMessage> SendMessageAsync(Snowflake channelId, LocalAttachment attachment, string content = null, bool textToSpeech = false, Embed embed = null, RestRequestOptions options = null);

        Task<RestUserMessage> SendMessageAsync(Snowflake channelId, IEnumerable<LocalAttachment> attachments, string content = null, bool textToSpeech = false, Embed embed = null, RestRequestOptions options = null);

        Task AddReactionAsync(Snowflake channelId, Snowflake messageId, IEmoji emoji, RestRequestOptions options = null);

        Task RemoveOwnReactionAsync(Snowflake channelId, Snowflake messageId, IEmoji emoji, RestRequestOptions options = null);

        Task RemoveMemberReactionAsync(Snowflake channelId, Snowflake messageId, Snowflake memberId, IEmoji emoji, RestRequestOptions options = null);

        RestRequestEnumerator<RestUser> GetReactionEnumerator(Snowflake channelId, Snowflake messageId, IEmoji emoji, int limit, RetrievalDirection? direction = null, Snowflake? startFromId = null);

        Task<IReadOnlyList<RestUser>> GetReactionsAsync(Snowflake channelId, Snowflake messageId, IEmoji emoji, int limit = 100, RetrievalDirection? direction = null, Snowflake? startFromId = null, RestRequestOptions options = null);

        Task RemoveAllReactionsAsync(Snowflake channelId, Snowflake messageId, RestRequestOptions options = null);

        Task<RestMessage> ModifyMessageAsync(Snowflake channelId, Snowflake messageId, Action<ModifyMessageProperties> action, RestRequestOptions options = null);

        Task DeleteMessageAsync(Snowflake channelId, Snowflake messageId, RestRequestOptions options = null);

        RestRequestEnumerator<Snowflake> GetBulkMessageDeletionEnumerator(Snowflake channelId, IEnumerable<Snowflake> messageIds);

        Task DeleteMessagesAsync(Snowflake channelId, IEnumerable<Snowflake> messageIds, RestRequestOptions options = null);

        Task AddOrModifyOverwriteAsync(Snowflake channelId, LocalOverwrite overwrite, RestRequestOptions options = null);

        Task<IReadOnlyList<RestInvite>> GetChannelInvitesAsync(Snowflake channelId, RestRequestOptions options = null);

        Task<RestInvite> CreateInviteAsync(Snowflake channelId, int maxAgeSeconds = 86400, int maxUses = 0, bool isTemporaryMembership = false, bool isUnique = false, RestRequestOptions options = null);

        Task<RestInvite> CreateInviteAsync(Snowflake channelId, TimeSpan maxAge, int maxUses = 0, bool isTemporaryMembership = false, bool isUnique = false, RestRequestOptions options = null);

        Task DeleteOverwriteAsync(Snowflake channelId, Snowflake targetId, RestRequestOptions options = null);

        Task TriggerTypingAsync(Snowflake channelId, RestRequestOptions options = null);

        Task<IReadOnlyList<RestUserMessage>> GetPinnedMessagesAsync(Snowflake channelId, RestRequestOptions options = null);

        Task PinMessageAsync(Snowflake channelId, Snowflake messageId, RestRequestOptions options = null);

        Task UnpinMessageAsync(Snowflake channelId, Snowflake messageId, RestRequestOptions options = null);

        Task AddGroupRecipientAsync(Snowflake channelId, Snowflake userId, string nick = null, string accessToken = null, RestRequestOptions options = null);

        Task RemoveGroupRecipientAsync(Snowflake channelId, Snowflake userId, RestRequestOptions options = null);
    }
}
