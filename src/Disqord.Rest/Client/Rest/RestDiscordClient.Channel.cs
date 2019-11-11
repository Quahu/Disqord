using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Disqord.Models;
using Qommon.Collections;

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

        public RestRequestEnumerator<RestMessage> GetMessagesEnumerator(Snowflake channelId, int limit, RetrievalDirection direction = RetrievalDirection.Before, Snowflake? startFromId = null)
        {
            var enumerator = new RestRequestEnumerator<RestMessage>();
            var remaining = limit;
            do
            {
                var amount = remaining > 100 ? 100 : remaining;
                remaining -= amount;
                enumerator.Enqueue(async (previous, options) =>
                {
                    var startFrom = startFromId;
                    if (previous != null && previous.Count > 0)
                    {
                        switch (direction)
                        {
                            case RetrievalDirection.Before:
                                startFrom = previous[previous.Count - 1].Id;
                                break;

                            case RetrievalDirection.After:
                                startFrom = previous[0].Id;
                                break;

                            case RetrievalDirection.Around:
                                throw new NotImplementedException();

                            default:
                                throw new ArgumentOutOfRangeException(nameof(direction));
                        }
                    }
                    var messages = await InternalGetMessagesAsync(channelId, amount, direction, startFrom, options).ConfigureAwait(false);
                    if (messages.Count < 100)
                        enumerator.Cancel();

                    return messages;
                });
            }
            while (remaining > 0);
            return enumerator;
        }

        public async Task<IReadOnlyList<RestMessage>> GetMessagesAsync(Snowflake channelId, int limit = 100, RetrievalDirection direction = RetrievalDirection.Before, Snowflake? startFromId = null, RestRequestOptions options = null)
        {
            if (limit == 0)
                return ImmutableArray<RestMessage>.Empty;

            if (limit <= 100)
                return await InternalGetMessagesAsync(channelId, limit, direction, startFromId, options).ConfigureAwait(false);

            var enumerator = GetMessagesEnumerator(channelId, limit, direction, startFromId);
            await using (enumerator.ConfigureAwait(false))
            {
                return await enumerator.FlattenAsync(options).ConfigureAwait(false);
            }
        }

        internal async Task<IReadOnlyList<RestMessage>> InternalGetMessagesAsync(Snowflake channelId, int limit = 100, RetrievalDirection direction = RetrievalDirection.Before, Snowflake? startFromId = null, RestRequestOptions options = null)
        {
            var models = await ApiClient.GetChannelMessagesAsync(channelId, limit, direction, startFromId, options).ConfigureAwait(false);
            return models.Select(x => RestMessage.Create(this, x)).ToImmutableArray();
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

        public async Task<RestUserMessage> SendMessageAsync(Snowflake channelId, string content = null, bool textToSpeech = false, LocalEmbed embed = null, RestRequestOptions options = null)
        {
            var model = await ApiClient.CreateMessageAsync(channelId, content, textToSpeech, embed, options).ConfigureAwait(false);
            return new RestUserMessage(this, model);
        }

        public async Task<RestUserMessage> SendMessageAsync(Snowflake channelId, LocalAttachment attachment, string content = null, bool textToSpeech = false, LocalEmbed embed = null, RestRequestOptions options = null)
        {
            if (attachment == null)
                throw new ArgumentNullException(nameof(attachment));

            var model = await ApiClient.CreateMessageAsync(channelId, attachment, content, textToSpeech, embed, options).ConfigureAwait(false);
            return new RestUserMessage(this, model);
        }

        public async Task<RestUserMessage> SendMessageAsync(Snowflake channelId, IEnumerable<LocalAttachment> attachments, string content = null, bool textToSpeech = false, LocalEmbed embed = null, RestRequestOptions options = null)
        {
            if (attachments == null)
                throw new ArgumentNullException(nameof(attachments));

            var model = await ApiClient.CreateMessageAsync(channelId, attachments, content, textToSpeech, embed, options).ConfigureAwait(false);
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

            var currentUser = await CurrentUser.GetOrDownloadAsync(options).ConfigureAwait(false);
            if (currentUser.Id == memberId)
                await ApiClient.DeleteOwnReactionAsync(channelId, messageId, emoji.ReactionFormat, options).ConfigureAwait(false);

            else
                await ApiClient.DeleteUserReactionAsync(channelId, messageId, memberId, emoji.ReactionFormat, options).ConfigureAwait(false);
        }

        public RestRequestEnumerator<RestUser> GetReactionEnumerator(Snowflake channelId, Snowflake messageId, IEmoji emoji, int limit, RetrievalDirection direction = RetrievalDirection.Before, Snowflake? startFromId = null)
        {
            if (emoji == null)
                throw new ArgumentNullException(nameof(emoji));

            var enumerator = new RestRequestEnumerator<RestUser>();
            var remaining = limit;
            do
            {
                var amount = remaining > 100 ? 100 : remaining;
                remaining -= amount;
                enumerator.Enqueue(async (previous, options) =>
                {
                    var startFrom = startFromId;
                    if (previous != null && previous.Count > 0)
                    {
                        switch (direction)
                        {
                            case RetrievalDirection.Before:
                                startFrom = previous[previous.Count - 1].Id;
                                break;

                            case RetrievalDirection.After:
                                startFrom = previous[0].Id;
                                break;

                            case RetrievalDirection.Around:
                                throw new NotSupportedException();

                            default:
                                throw new ArgumentOutOfRangeException(nameof(direction));
                        }
                    }
                    var users = await InternalGetReactionsAsync(channelId, messageId, emoji, amount, direction, startFrom, options).ConfigureAwait(false);
                    if (users.Count != 100)
                        enumerator.Cancel();

                    return users;
                });
            }
            while (remaining > 0);
            return enumerator;
        }

        public async Task<IReadOnlyList<RestUser>> GetReactionsAsync(Snowflake channelId, Snowflake messageId, IEmoji emoji, int limit = 100, RetrievalDirection direction = RetrievalDirection.Before, Snowflake? startFromId = null, RestRequestOptions options = null)
        {
            if (emoji == null)
                throw new ArgumentNullException(nameof(emoji));

            if (limit == 0)
                return ImmutableArray<RestUser>.Empty;

            if (limit <= 100)
                return await InternalGetReactionsAsync(channelId, messageId, emoji, limit, direction, startFromId).ConfigureAwait(false);

            var enumerator = GetReactionEnumerator(channelId, messageId, emoji, limit, direction, startFromId);
            await using (enumerator.ConfigureAwait(false))
            {
                return await enumerator.FlattenAsync(options).ConfigureAwait(false);
            }
        }

        internal async Task<IReadOnlyList<RestUser>> InternalGetReactionsAsync(Snowflake channelId, Snowflake messageId, IEmoji emoji, int limit = 100, RetrievalDirection direction = RetrievalDirection.Before, Snowflake? startFromId = null, RestRequestOptions options = null)
        {
            var models = await ApiClient.GetReactionsAsync(channelId, messageId, emoji.ReactionFormat, limit, direction, startFromId, options).ConfigureAwait(false);
            return models.Select(x => new RestUser(this, x)).ToImmutableArray();
        }

        public Task ClearReactionsAsync(Snowflake channelId, Snowflake messageId, RestRequestOptions options = null)
            => ApiClient.DeleteAllReactionsAsync(channelId, messageId, options);

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

        public RestRequestEnumerator<Snowflake> GetBulkMessageDeletionEnumerator(Snowflake channelId, IEnumerable<Snowflake> messageIds)
        {
            if (messageIds == null)
                throw new ArgumentNullException(nameof(messageIds));

            var messages = messageIds.ToArray();
            if (messages.Length == 0)
                return new RestRequestEnumerator<Snowflake>();

            return InternalGetBulkMessageDeletionEnumerator(channelId, messages);
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

            var enumerator = InternalGetBulkMessageDeletionEnumerator(channelId, messages);
            await using (enumerator.ConfigureAwait(false))
            {
                await enumerator.FlattenAsync(options).ConfigureAwait(false);
            }
        }

        internal RestRequestEnumerator<Snowflake> InternalGetBulkMessageDeletionEnumerator(Snowflake channelId, Snowflake[] messageIds)
        {
            var enumerator = new RestRequestEnumerator<Snowflake>();
            var remaining = messageIds.Length;
            var offset = 0;
            do
            {
                var amount = remaining > 100 ? 100 : remaining;
                var segment = new ArraySegment<Snowflake>(messageIds, offset, amount);
                enumerator.Enqueue(async (_, options) =>
                {
                    if (amount == 1)
                        await DeleteMessageAsync(channelId, segment[0], options).ConfigureAwait(false);
                    else
                        await ApiClient.BulkDeleteMessagesAsync(channelId, segment.Select(x => x.RawValue), options).ConfigureAwait(false);

                    return new ReadOnlyList<Snowflake>(segment);
                });
                remaining -= amount;
                offset += amount;
            }
            while (remaining > 0);
            return enumerator;
        }

        public Task AddOrModifyOverwriteAsync(Snowflake channelId, LocalOverwrite overwrite, RestRequestOptions options = null)
            => ApiClient.EditChannelPermissionsAsync(channelId, overwrite, options);

        public async Task<IReadOnlyList<RestInvite>> GetChannelInvitesAsync(Snowflake channelId, RestRequestOptions options = null)
        {
            var models = await ApiClient.GetChannelInvitesAsync(channelId, options).ConfigureAwait(false);
            return models.Select(x => new RestInvite(this, x)).ToImmutableArray();
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
            return models.Select(x => new RestUserMessage(this, x)).ToImmutableArray();
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
