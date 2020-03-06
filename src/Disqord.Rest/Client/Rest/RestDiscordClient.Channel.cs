using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Disqord.Collections;
using Disqord.Models;

namespace Disqord.Rest
{
    public partial class RestDiscordClient : IRestDiscordClient
    {
        public async Task<RestChannel> GetChannelAsync(Snowflake channelId, RestRequestOptions options = null)
        {
            try
            {
                var model = await ApiClient.GetChannelAsync(channelId, options).ConfigureAwait(false);
                return RestChannel.Create(this, model);
            }
            catch (DiscordHttpException ex) when (ex.HttpStatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<T> GetChannelAsync<T>(Snowflake channelId, RestRequestOptions options = null) where T : RestChannel
        {
            try
            {
                var model = await ApiClient.GetChannelAsync(channelId, options).ConfigureAwait(false);
                return (T) RestChannel.Create(this, model);
            }
            catch (DiscordHttpException ex) when (ex.HttpStatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<RestGroupChannel> ModifyGroupChannelAsync(Snowflake channelId, Action<ModifyGroupChannelProperties> action, RestRequestOptions options = null)
        {
            var model = await InternalModifyChannelAsync(channelId, action, options).ConfigureAwait(false);
            return new RestGroupChannel(this, model);
        }

        public async Task<RestTextChannel> ModifyTextChannelAsync(Snowflake channelId, Action<ModifyTextChannelProperties> action, RestRequestOptions options = null)
        {
            var model = await InternalModifyChannelAsync(channelId, action, options).ConfigureAwait(false);
            return new RestTextChannel(this, model);
        }

        public async Task<RestVoiceChannel> ModifyVoiceChannelAsync(Snowflake channelId, Action<ModifyVoiceChannelProperties> action, RestRequestOptions options = null)
        {
            var model = await InternalModifyChannelAsync(channelId, action, options).ConfigureAwait(false);
            return new RestVoiceChannel(this, model);
        }

        public async Task<RestCategoryChannel> ModifyCategoryChannelAsync(Snowflake channelId, Action<ModifyCategoryChannelProperties> action, RestRequestOptions options = null)
        {
            var model = await InternalModifyChannelAsync(channelId, action, options).ConfigureAwait(false);
            return new RestCategoryChannel(this, model);
        }

        internal async Task<ChannelModel> InternalModifyChannelAsync<T>(Snowflake channelId, Action<T> action, RestRequestOptions options = null)
            where T : ModifyChannelProperties
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            // Can't use the new() generic constraint because the constructors are internal.
            // Can't use the generic CreateInstance either *because*.
            var properties = (T) Activator.CreateInstance(typeof(T), true);
            action(properties);
            return await ApiClient.ModifyChannelAsync(channelId, properties, options).ConfigureAwait(false);
        }

        public Task DeleteOrCloseChannelAsync(Snowflake channelId, RestRequestOptions options = null)
            => ApiClient.DeleteOrCloseChannelAsync(channelId, options);

        public RestRequestEnumerable<RestMessage> GetMessagesEnumerable(Snowflake channelId, int limit, RetrievalDirection direction = RetrievalDirection.Before, Snowflake? startFromId = null, RestRequestOptions options = null)
            => new RestRequestEnumerable<RestMessage>(new RestMessagesRequestEnumerator(this, channelId, limit, direction, startFromId, options));

        public Task<IReadOnlyList<RestMessage>> GetMessagesAsync(Snowflake channelId, int limit = 100, RetrievalDirection direction = RetrievalDirection.Before, Snowflake? startFromId = null, RestRequestOptions options = null)
        {
            if (limit == 0)
                return Task.FromResult<IReadOnlyList<RestMessage>>(ReadOnlyList<RestMessage>.Empty);

            if (limit <= 100)
                return InternalGetMessagesAsync(channelId, limit, direction, startFromId, options);

            var enumerable = GetMessagesEnumerable(channelId, limit, direction, startFromId, options);
            return enumerable.FlattenAsync();
        }

        internal async Task<IReadOnlyList<RestMessage>> InternalGetMessagesAsync(Snowflake channelId, int limit = 100, RetrievalDirection direction = RetrievalDirection.Before, Snowflake? startFromId = null, RestRequestOptions options = null)
        {
            var models = await ApiClient.GetChannelMessagesAsync(channelId, limit, direction, startFromId, options).ConfigureAwait(false);
            return models.ToReadOnlyList(this, (x, @this) => RestMessage.Create(@this, x));
        }

        public async Task<RestMessage> GetMessageAsync(Snowflake channelId, Snowflake messageId, RestRequestOptions options = null)
        {
            try
            {
                var model = await ApiClient.GetChannelMessageAsync(channelId, messageId, options).ConfigureAwait(false);
                return RestMessage.Create(this, model);
            }
            catch (DiscordHttpException ex) when (ex.HttpStatusCode == HttpStatusCode.NotFound && ex.JsonErrorCode == JsonErrorCode.UnknownMessage)
            {
                return null;
            }
        }

        public async Task<RestUserMessage> SendMessageAsync(Snowflake channelId, string content = null, bool textToSpeech = false, LocalEmbed embed = null, LocalMentions mentions = null, RestRequestOptions options = null)
        {
            var model = await ApiClient.CreateMessageAsync(channelId, content, textToSpeech, embed, mentions, options).ConfigureAwait(false);
            return new RestUserMessage(this, model);
        }

        public async Task<RestUserMessage> SendMessageAsync(Snowflake channelId, LocalAttachment attachment, string content = null, bool textToSpeech = false, LocalEmbed embed = null, LocalMentions mentions = null, RestRequestOptions options = null)
        {
            if (attachment == null)
                throw new ArgumentNullException(nameof(attachment));

            var model = await ApiClient.CreateMessageAsync(channelId, attachment, content, textToSpeech, embed, mentions, options).ConfigureAwait(false);
            return new RestUserMessage(this, model);
        }

        public async Task<RestUserMessage> SendMessageAsync(Snowflake channelId, IEnumerable<LocalAttachment> attachments, string content = null, bool textToSpeech = false, LocalEmbed embed = null, LocalMentions mentions = null, RestRequestOptions options = null)
        {
            if (attachments == null)
                throw new ArgumentNullException(nameof(attachments));

            var model = await ApiClient.CreateMessageAsync(channelId, attachments, content, textToSpeech, embed, mentions, options).ConfigureAwait(false);
            return new RestUserMessage(this, model);
        }

        public Task AddReactionAsync(Snowflake channelId, Snowflake messageId, IEmoji emoji, RestRequestOptions options = null)
        {
            if (emoji == null)
                throw new ArgumentNullException(nameof(emoji));

            return ApiClient.CreateReactionAsync(channelId, messageId, emoji.ReactionFormat, options);
        }

        public async Task RemoveOwnReactionAsync(Snowflake channelId, Snowflake messageId, IEmoji emoji, RestRequestOptions options = null)
        {
            if (emoji == null)
                throw new ArgumentNullException(nameof(emoji));

            await ApiClient.DeleteOwnReactionAsync(channelId, messageId, emoji.ReactionFormat, options).ConfigureAwait(false);
        }

        public async Task RemoveMemberReactionAsync(Snowflake channelId, Snowflake messageId, Snowflake memberId, IEmoji emoji, RestRequestOptions options = null)
        {
            if (emoji == null)
                throw new ArgumentNullException(nameof(emoji));

            var currentUser = await CurrentUser.GetAsync(options).ConfigureAwait(false);
            if (currentUser.Id == memberId)
                await ApiClient.DeleteOwnReactionAsync(channelId, messageId, emoji.ReactionFormat, options).ConfigureAwait(false);

            else
                await ApiClient.DeleteUserReactionAsync(channelId, messageId, memberId, emoji.ReactionFormat, options).ConfigureAwait(false);
        }

        public RestRequestEnumerable<RestUser> GetReactionsEnumerable(Snowflake channelId, Snowflake messageId, IEmoji emoji, int limit, Snowflake? startFromId = null, RestRequestOptions options = null)
        {
            if (emoji == null)
                throw new ArgumentNullException(nameof(emoji));

            return new RestRequestEnumerable<RestUser>(new RestReactionsRequestEnumerator(this, channelId, messageId, emoji, limit, startFromId, options));
        }

        public Task<IReadOnlyList<RestUser>> GetReactionsAsync(Snowflake channelId, Snowflake messageId, IEmoji emoji, int limit = 100, Snowflake? startFromId = null, RestRequestOptions options = null)
        {
            if (emoji == null)
                throw new ArgumentNullException(nameof(emoji));

            if (limit == 0)
                return Task.FromResult(ReadOnlyList<RestUser>.Empty);

            if (limit <= 100)
                return InternalGetReactionsAsync(channelId, messageId, emoji, limit, startFromId, options);

            var enumerable = GetReactionsEnumerable(channelId, messageId, emoji, limit, startFromId, options);
            return enumerable.FlattenAsync();
        }

        internal async Task<IReadOnlyList<RestUser>> InternalGetReactionsAsync(Snowflake channelId, Snowflake messageId, IEmoji emoji, int limit = 100, Snowflake? startFromId = null, RestRequestOptions options = null)
        {
            var models = await ApiClient.GetReactionsAsync(channelId, messageId, emoji.ReactionFormat, limit, startFromId, options).ConfigureAwait(false);
            return models.ToReadOnlyList(this, (x, @this) => new RestUser(@this, x));
        }

        public Task ClearReactionsAsync(Snowflake channelId, Snowflake messageId, IEmoji emoji = null, RestRequestOptions options = null)
            => emoji != null
                ? ApiClient.DeleteAllReactionsForEmojiAsync(channelId, messageId, emoji.ReactionFormat, options)
                : ApiClient.DeleteAllReactionsAsync(channelId, messageId, options);

        public async Task<RestUserMessage> ModifyMessageAsync(Snowflake channelId, Snowflake messageId, Action<ModifyMessageProperties> action, RestRequestOptions options = null)
        {
            var model = await InternalModifyMessageAsync(channelId, messageId, action, options).ConfigureAwait(false);
            return new RestUserMessage(this, model);
        }

        internal async Task<MessageModel> InternalModifyMessageAsync(Snowflake channelId, Snowflake messageId, Action<ModifyMessageProperties> action, RestRequestOptions options)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var properties = new ModifyMessageProperties();
            action(properties);
            return await ApiClient.EditMessageAsync(channelId, messageId, properties, options).ConfigureAwait(false);
        }

        public Task DeleteMessageAsync(Snowflake channelId, Snowflake messageId, RestRequestOptions options = null)
            => ApiClient.DeleteMessageAsync(channelId, messageId, options);

        public RestRequestEnumerator<Snowflake> GetBulkMessageDeletionEnumerator(Snowflake channelId, IEnumerable<Snowflake> messageIds, RestRequestOptions options)
        {
            if (messageIds == null)
                throw new ArgumentNullException(nameof(messageIds));

            var messages = messageIds.ToArray();
            return InternalGetBulkMessageDeletionEnumerator(channelId, messages, options);
        }

        public async Task DeleteMessagesAsync(Snowflake channelId, IEnumerable<Snowflake> messageIds, RestRequestOptions options = null)
        {
            if (messageIds == null)
                throw new ArgumentNullException(nameof(messageIds));

            var messages = messageIds.ToArray();
            if (messages.Length == 0)
                return;

            if (messages.Length == 1)
            {
                await DeleteMessageAsync(channelId, messages[0], options).ConfigureAwait(false);
                return;
            }

            if (messages.Length <= 100)
            {
                await ApiClient.BulkDeleteMessagesAsync(channelId, messages.Select(x => x.RawValue), options).ConfigureAwait(false);
                return;
            }

            var enumerator = InternalGetBulkMessageDeletionEnumerator(channelId, messages, options);
            await using (enumerator.ConfigureAwait(false))
            {
                // Exhaust the enumerator.
                while (await enumerator.MoveNextAsync().ConfigureAwait(false))
                { }
            }
        }

        internal RestRequestEnumerator<Snowflake> InternalGetBulkMessageDeletionEnumerator(Snowflake channelId, Snowflake[] messageIds, RestRequestOptions options)
            => new RestBulkDeleteMessagesRequestEnumerator(this, channelId, messageIds, options);

        public Task AddOrModifyOverwriteAsync(Snowflake channelId, LocalOverwrite overwrite, RestRequestOptions options = null)
            => ApiClient.EditChannelPermissionsAsync(channelId, overwrite, options);

        public async Task<IReadOnlyList<RestInvite>> GetChannelInvitesAsync(Snowflake channelId, RestRequestOptions options = null)
        {
            var models = await ApiClient.GetChannelInvitesAsync(channelId, options).ConfigureAwait(false);
            return models.ToReadOnlyList(this, (x, @this) => new RestInvite(@this, x));
        }

        public async Task<RestInvite> CreateInviteAsync(Snowflake channelId, int maxAgeSeconds = 86400, int maxUses = 0, bool isTemporaryMembership = false, bool isUnique = false, RestRequestOptions options = null)
        {
            var model = await ApiClient.CreateInviteAsync(channelId, maxAgeSeconds, maxUses, isTemporaryMembership, isUnique, options).ConfigureAwait(false);
            return new RestInvite(this, model);
        }

        public Task<RestInvite> CreateInviteAsync(Snowflake channelId, TimeSpan maxAge, int maxUses = 0, bool isTemporaryMembership = false, bool isUnique = false, RestRequestOptions options = null)
            => CreateInviteAsync(channelId, (int) maxAge.TotalSeconds, maxUses, isTemporaryMembership, isUnique, options);

        public Task DeleteOverwriteAsync(Snowflake channelId, Snowflake targetId, RestRequestOptions options = null)
            => ApiClient.DeleteChannelPermissionAsync(channelId, targetId, options);

        public Task TriggerTypingAsync(Snowflake channelId, RestRequestOptions options = null)
            => ApiClient.TriggerTypingIndicatorAsync(channelId, options);

        public async Task<IReadOnlyList<RestUserMessage>> GetPinnedMessagesAsync(Snowflake channelId, RestRequestOptions options = null)
        {
            var models = await ApiClient.GetPinnedMessagesAsync(channelId, options).ConfigureAwait(false);
            return models.ToReadOnlyList(this, (x, @this) => new RestUserMessage(@this, x));
        }

        public Task PinMessageAsync(Snowflake channelId, Snowflake messageId, RestRequestOptions options = null)
            => ApiClient.AddPinnedMessageAsync(channelId, messageId, options);

        public Task UnpinMessageAsync(Snowflake channelId, Snowflake messageId, RestRequestOptions options = null)
            => ApiClient.DeletePinnedMessageAsync(channelId, messageId, options);

        public Task AddGroupRecipientAsync(Snowflake channelId, Snowflake userId, string nick = null, string accessToken = null, RestRequestOptions options = null)
            => ApiClient.GroupDmRecipientAddAsync(channelId, userId, nick, accessToken, options);

        public Task RemoveGroupRecipientAsync(Snowflake channelId, Snowflake userId, RestRequestOptions options = null)
            => ApiClient.GroupDmRecipientRemoveAsync(channelId, userId, options);
    }
}
