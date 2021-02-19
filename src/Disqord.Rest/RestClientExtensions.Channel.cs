﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Collections;
using Disqord.Http;
using Disqord.Models;
using Disqord.Rest.Api;
using Disqord.Rest.Models;
using Disqord.Rest.Pagination;
using Disqord.Rest.Repetition;

namespace Disqord.Rest
{
    public static partial class RestClientExtensions
    {
        public static async Task<IChannel> FetchChannelAsync(this IRestClient client, Snowflake channelId, IRestRequestOptions options = null)
        {
            try
            {
                var model = await client.ApiClient.FetchChannelAsync(channelId, options).ConfigureAwait(false);
                return TransientChannel.Create(client, model);
            }
            catch (RestApiException ex) when (ex.StatusCode == HttpResponseStatusCode.NotFound)
            {
                return null;
            }
        }

        public static async Task<ITextChannel> ModifyTextChannelAsync(this IRestClient client, Snowflake channelId, Action<ModifyTextChannelActionProperties> action, IRestRequestOptions options = null)
        {
            var model = await client.InternalModifyChannelAsync(channelId, action, options).ConfigureAwait(false);
            return new TransientTextChannel(client, model);
        }

        public static async Task<IVoiceChannel> ModifyVoiceChannelAsync(this IRestClient client, Snowflake channelId, Action<ModifyVoiceChannelActionProperties> action, IRestRequestOptions options = null)
        {
            var model = await client.InternalModifyChannelAsync(channelId, action, options).ConfigureAwait(false);
            return new TransientVoiceChannel(client, model);
        }

        public static async Task<ICategoryChannel> ModifyCategoryChannelAsync(this IRestClient client, Snowflake channelId, Action<ModifyCategoryChannelActionProperties> action, IRestRequestOptions options = null)
        {
            var model = await client.InternalModifyChannelAsync(channelId, action, options).ConfigureAwait(false);
            return new TransientCategoryChannel(client, model);
        }

        internal static Task<ChannelJsonModel> InternalModifyChannelAsync<T>(this IRestClient client, Snowflake channelId, Action<T> action, IRestRequestOptions options = null)
            where T : ModifyChannelActionProperties
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            // Can't use the new() generic constraint because the constructors are internal.
            // Can't use the generic CreateInstance either *because*.
            var properties = (T) Activator.CreateInstance(typeof(T), true);
            action(properties);
            var content = new ModifyChannelJsonRestRequestContent
            {
                Name = properties.Name
            };
            if (properties is ModifyGuildChannelActionProperties guildProperties)
            {
                content.Position = guildProperties.Position;
                content.PermissionOverwrites = Optional.Convert(guildProperties.Overwrites, x => x.Select(x => x.ToModel()).ToArray());

                if (guildProperties is ModifyNestableChannelActionProperties nestableProperties)
                {
                    content.ParentId = nestableProperties.CategoryId;

                    if (nestableProperties is ModifyTextChannelActionProperties textProperties)
                    {
                        content.Topic = textProperties.Topic;
                        content.Nsfw = textProperties.IsNsfw;
                        content.RateLimitPerUser = textProperties.Slowmode;
                    }
                    else if (nestableProperties is ModifyVoiceChannelActionProperties voiceProperties)
                    {
                        content.Bitrate = voiceProperties.Bitrate;
                        content.UserLimit = voiceProperties.UserLimit;
                    }
                }
                else if (guildProperties is ModifyCategoryChannelActionProperties categoryProperties)
                {
                    // No extra properties for category channels.
                }
            }
            else
            {
                throw new ArgumentException($"Unknown channel action properties provided ({properties.GetType()}).");
            }

            return client.ApiClient.ModifyChannelAsync(channelId, content, options);
        }

        public static Task DeleteChannelAsync(this IRestClient client, Snowflake channelId, IRestRequestOptions options = null)
            => client.ApiClient.DeleteChannelAsync(channelId, options);

        public static IPagedEnumerable<IMessage> EnumerateMessages(this IRestClient client, Snowflake channelId, int limit, RetrievalDirection direction = RetrievalDirection.Before, Snowflake? startFromId = null, IRestRequestOptions options = null)
            => new PagedEnumerable<IMessage>(new FetchMessagesPagedEnumerator(client, channelId, limit, direction, startFromId, options));

        public static Task<IReadOnlyList<IMessage>> FetchMessagesAsync(this IRestClient client, Snowflake channelId, int limit = 100, RetrievalDirection direction = RetrievalDirection.Before, Snowflake? startFromId = null, IRestRequestOptions options = null)
        {
            if (limit == 0)
                return Task.FromResult(ReadOnlyList<IMessage>.Empty);

            if (limit <= 100)
                return client.InternalFetchMessagesAsync(channelId, limit, direction, startFromId, options);

            var enumerable = client.EnumerateMessages(channelId, limit, direction, startFromId, options);
            return enumerable.FlattenAsync();
        }

        internal static async Task<IReadOnlyList<IMessage>> InternalFetchMessagesAsync(this IRestClient client, Snowflake channelId, int limit = 100, RetrievalDirection direction = RetrievalDirection.Before, Snowflake? startFromId = null, IRestRequestOptions options = null)
        {
            var models = await client.ApiClient.FetchMessagesAsync(channelId, limit, direction, startFromId, options).ConfigureAwait(false);
            return models.ToReadOnlyList(client, (x, client) => TransientMessage.Create(client, x));
        }

        public static async Task<IMessage> FetchMessageAsync(this IRestClient client, Snowflake channelId, Snowflake messageId, IRestRequestOptions options = null)
        {
            try
            {
                var model = await client.ApiClient.FetchMessageAsync(channelId, messageId, options).ConfigureAwait(false);
                return TransientMessage.Create(client, model);
            }
            catch (RestApiException ex) when (ex.StatusCode == HttpResponseStatusCode.NotFound && ex.ErrorModel.Code == RestApiErrorCode.UnknownMessage)
            {
                return null;
            }
        }

        public static async Task<IUserMessage> SendMessageAsync(this IRestClient client, Snowflake channelId, LocalMessage message, IRestRequestOptions options = null)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            var messageContent = new CreateMessageJsonRestRequestContent
            {
                Content = Optional.FromNullable(message.Content),
                Tts = message.IsTextToSpeech,
                Embed = Optional.FromNullable(message.Embed.ToModel()),
                AllowedMentions = Optional.FromNullable(message.Mentions.ToModel()),
                Nonce = Optional.FromNullable(message.Nonce)
            };

            Task<MessageJsonModel> task;
            if (message.Attachments.Count != 0)
            {
                // If there are attachments, we must send them via multipart HTTP content.
                // Our `messageContent` will be serialized into a form data field.
                var content = new MultipartJsonPayloadRestRequestContent<CreateMessageJsonRestRequestContent>(messageContent, message.Attachments);
                task = client.ApiClient.CreateMessageAsync(channelId, content, options);
            }
            else
            {
                task = client.ApiClient.CreateMessageAsync(channelId, messageContent, options);
            }

            var model = await task.ConfigureAwait(false);
            return new TransientUserMessage(client, model);
        }

        // TODO: crosspost message

        public static Task CreateReactionAsync(this IRestClient client, Snowflake channelId, Snowflake messageId, IEmoji emoji, IRestRequestOptions options = null)
        {
            if (emoji == null)
                throw new ArgumentNullException(nameof(emoji));

            return client.ApiClient.CreateReactionAsync(channelId, messageId, Discord.GetReactionFormat(emoji), options);
        }

        public static Task DeleteOwnReactionAsync(this IRestClient client, Snowflake channelId, Snowflake messageId, IEmoji emoji, IRestRequestOptions options = null)
        {
            if (emoji == null)
                throw new ArgumentNullException(nameof(emoji));

            return client.ApiClient.DeleteOwnReactionAsync(channelId, messageId, Discord.GetReactionFormat(emoji), options);
        }

        public static Task DeleteUserReactionAsync(this IRestClient client, Snowflake channelId, Snowflake messageId, IEmoji emoji, Snowflake userId, IRestRequestOptions options = null)
        {
            if (emoji == null)
                throw new ArgumentNullException(nameof(emoji));

            var reactionFormat = Discord.GetReactionFormat(emoji);
            if (client.ApiClient.Token is BotToken botToken && botToken.Id == userId)
                return client.ApiClient.DeleteOwnReactionAsync(channelId, messageId, reactionFormat, options);
            else
                return client.ApiClient.DeleteUserReactionAsync(channelId, messageId, reactionFormat, userId, options);
        }

        public static IPagedEnumerable<IUser> EnumerateReactions(this IRestClient client, Snowflake channelId, Snowflake messageId, IEmoji emoji, int limit, RetrievalDirection direction = RetrievalDirection.Before, Snowflake? startFromId = null, IRestRequestOptions options = null)
        {
            if (emoji == null)
                throw new ArgumentNullException(nameof(emoji));

            return new PagedEnumerable<IUser>(new FetchReactionsPagedEnumerator(client, channelId, messageId, emoji, limit, direction, startFromId, options));
        }

        public static Task<IReadOnlyList<IUser>> FetchReactionsAsync(this IRestClient client, Snowflake channelId, Snowflake messageId, IEmoji emoji, int limit = 100, RetrievalDirection direction = RetrievalDirection.Before, Snowflake? startFromId = null, IRestRequestOptions options = null)
        {
            if (emoji == null)
                throw new ArgumentNullException(nameof(emoji));

            if (limit == 0)
                return Task.FromResult(ReadOnlyList<IUser>.Empty);

            if (limit <= 100)
                return client.InternalFetchReactionsAsync(channelId, messageId, emoji, limit, direction, startFromId, options);

            var enumerable = client.EnumerateReactions(channelId, messageId, emoji, limit, direction, startFromId, options);
            return enumerable.FlattenAsync();
        }

        internal static async Task<IReadOnlyList<IUser>> InternalFetchReactionsAsync(this IRestClient client, Snowflake channelId, Snowflake messageId, IEmoji emoji, int limit = 100, RetrievalDirection direction = RetrievalDirection.Before, Snowflake? startFromId = null, IRestRequestOptions options = null)
        {
            var models = await client.ApiClient.FetchReactionsAsync(channelId, messageId, Discord.GetReactionFormat(emoji), limit, direction, startFromId, options).ConfigureAwait(false);
            return models.ToReadOnlyList(client, (x, client) => new TransientUser(client, x));
        }

        public static Task ClearReactionsAsync(this IRestClient client, Snowflake channelId, Snowflake messageId, IEmoji emoji = null, IRestRequestOptions options = null)
        {
            if (emoji == null)
                return client.ApiClient.ClearReactionsAsync(channelId, messageId, options);

            var reactionFormat = Discord.GetReactionFormat(emoji);
            return client.ApiClient.ClearEmojiReactionsAsync(channelId, messageId, reactionFormat, options);
        }

        public static async Task<IUserMessage> ModifyMessageAsync(this IRestClient client, Snowflake channelId, Snowflake messageId, Action<ModifyMessageActionProperties> action, IRestRequestOptions options = null)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var properties = new ModifyMessageActionProperties();
            action(properties);
            var content = new ModifyMessageJsonRestRequestContent
            {
                Content = properties.Content,
                Embed = Optional.Convert(properties.Embed, x => x.ToModel()),
                Flags = properties.Flags
            };
            var model = await client.ApiClient.ModifyMessageAsync(channelId, messageId, content, options).ConfigureAwait(false);
            return new TransientUserMessage(client, model);
        }

        public static Task DeleteMessageAsync(this IRestClient client, Snowflake channelId, Snowflake messageId, IRestRequestOptions options = null)
            => client.ApiClient.DeleteMessageAsync(channelId, messageId, options);

        public static IPagedEnumerable<Snowflake> EnumerateMessageDeletion(this IRestClient client, Snowflake channelId, IEnumerable<Snowflake> messageIds, IRestRequestOptions options = null)
            => new PagedEnumerable<Snowflake>(new DeleteMessagesPagedEnumerator(client, channelId, messageIds.ToArray(), options));

        public static async Task DeleteMessagesAsync(this IRestClient client, Snowflake channelId, IEnumerable<Snowflake> messageIds, IRestRequestOptions options = null)
        {
            if (messageIds == null)
                throw new ArgumentNullException(nameof(messageIds));

            var messages = messageIds.ToArray();
            if (messages.Length == 0)
                return;

            if (messages.Length == 1)
            {
                await client.DeleteMessageAsync(channelId, messages[0], options).ConfigureAwait(false);
                return;
            }

            if (messages.Length <= 100)
            {
                await client.DeleteMessagesAsync(channelId, messages, options).ConfigureAwait(false);
                return;
            }

            var enumerator = client.EnumerateMessageDeletion(channelId, messages, options).GetAsyncEnumerator();
            await using (enumerator.ConfigureAwait(false))
            {
                // Exhaust the enumerator.
                while (await enumerator.MoveNextAsync().ConfigureAwait(false))
                { }
            }
        }

        internal static Task InternalDeleteMessagesAsync(this IRestClient client, Snowflake channelId, ArraySegment<Snowflake> messageIds, IRestRequestOptions options = null)
        {
            var content = new DeleteMessagesJsonRestRequestContent
            {
                Messages = messageIds
            };
            return client.ApiClient.DeleteMessagesAsync(channelId, content, options);
        }

        public static Task SetOverwriteAsync(this IRestClient client, Snowflake channelId, LocalOverwrite overwrite, IRestRequestOptions options = null)
        {
            var content = new SetOverwriteJsonRestRequestContent
            {
                Type = overwrite.TargetType,
                Allow = overwrite.Permissions.Allowed,
                Deny = overwrite.Permissions.Denied
            };
            return client.ApiClient.SetOverwriteAsync(channelId, overwrite.TargetId, content, options);
        }

        public static Task DeleteOverwriteAsync(this IRestClient client, Snowflake channelId, Snowflake targetId, IRestRequestOptions options = null)
            => client.ApiClient.DeleteOverwriteAsync(channelId, targetId, options);

        public static async Task<IReadOnlyList<IInvite>> GetChannelInvitesAsync(this IRestClient client, Snowflake channelId, IRestRequestOptions options = null)
        {
            var models = await client.ApiClient.FetchChannelInvitesAsync(channelId, options).ConfigureAwait(false);
            return models.ToReadOnlyList(client, (x, client) => new TransientInvite(client, x));
        }

        /// <summary>
        ///     Creates a channel invite.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="channelId"></param>
        /// <param name="maxAge"> The max age of the invite. Defaults to 86400 seconds (24 hours). Set this to 0 for an indefinite invite. </param>
        /// <param name="maxUses"></param>
        /// <param name="isTemporaryMembership"></param>
        /// <param name="isUnique"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static async Task<IInvite> CreateInviteAsync(this IRestClient client, Snowflake channelId, TimeSpan maxAge = default, int maxUses = 0, bool isTemporaryMembership = false, bool isUnique = false, IRestRequestOptions options = null)
        {
            var content = new CreateChannelInviteJsonRestRequestContent
            {
                MaxAge = maxAge != default
                    ? (int) maxAge.TotalSeconds
                    : 86400,
                MaxUses = maxUses,
                Temporary = isTemporaryMembership,
                Unique = isUnique
            };
            var model = await client.ApiClient.CreateChannelInviteAsync(channelId, content, options).ConfigureAwait(false);
            return new TransientInvite(client, model);
        }

        // TODO: follow news channel

        public static Task TriggerTypingAsync(this IRestClient client, Snowflake channelId, IRestRequestOptions options = null)
            => client.ApiClient.TriggerTypingAsync(channelId, options);

        /// <summary>
        ///     Begins typing in the specified channel.
        /// </summary>
        /// <param name="client"> The REST client. </param>
        /// <param name="channelId"> The channel ID to type in. </param>
        /// <param name="options"> The optional request options. </param>
        /// <returns>
        ///     A disposable object that stops the typing.
        /// </returns>
        public static IDisposable BeginTyping(this IRestClient client, Snowflake channelId, IRestRequestOptions options = null)
            => new TypingRepeater(client, channelId, options);

        public static async Task<IReadOnlyList<IUserMessage>> FetchPinnedMessagesAsync(this IRestClient client, Snowflake channelId, IRestRequestOptions options = null)
        {
            var models = await client.ApiClient.FetchPinnedMessagesAsync(channelId, options).ConfigureAwait(false);
            return models.ToReadOnlyList(client, (x, client) => new TransientUserMessage(client, x));
        }

        public static Task PinMessageAsync(this IRestClient client, Snowflake channelId, Snowflake messageId, IRestRequestOptions options = null)
            => client.ApiClient.PinMessageAsync(channelId, messageId, options);

        public static Task UnpinMessageAsync(this IRestClient client, Snowflake channelId, Snowflake messageId, IRestRequestOptions options = null)
            => client.ApiClient.UnpinMessageAsync(channelId, messageId, options);
    }
}