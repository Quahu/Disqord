using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Http;
using Disqord.Models;
using Disqord.Rest.Api;
using Disqord.Rest.Pagination;
using Disqord.Rest.Repetition;
using Qommon;
using Qommon.Collections;
using Qommon.Collections.ReadOnly;

namespace Disqord.Rest
{
    public static partial class RestClientExtensions
    {
        public static async Task<IChannel> FetchChannelAsync(this IRestClient client,
            Snowflake channelId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var model = await client.ApiClient.FetchChannelAsync(channelId, options, cancellationToken).ConfigureAwait(false);
                return TransientChannel.Create(client, model);
            }
            catch (RestApiException ex) when (ex.StatusCode == HttpResponseStatusCode.NotFound)
            {
                return null;
            }
        }

        public static async Task<ITextChannel> ModifyTextChannelAsync(this IRestClient client,
            Snowflake channelId, Action<ModifyTextChannelActionProperties> action,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            Guard.IsNotNull(action);

            var model = await client.InternalModifyChannelAsync(channelId, action, options, cancellationToken).ConfigureAwait(false);
            return new TransientTextChannel(client, model);
        }

        public static async Task<IVoiceChannel> ModifyVoiceChannelAsync(this IRestClient client,
            Snowflake channelId, Action<ModifyVoiceChannelActionProperties> action,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            Guard.IsNotNull(action);

            var model = await client.InternalModifyChannelAsync(channelId, action, options, cancellationToken).ConfigureAwait(false);
            return new TransientVoiceChannel(client, model);
        }

        public static async Task<ICategoryChannel> ModifyCategoryChannelAsync(this IRestClient client,
            Snowflake channelId, Action<ModifyCategoryChannelActionProperties> action,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            Guard.IsNotNull(action);

            var model = await client.InternalModifyChannelAsync(channelId, action, options, cancellationToken).ConfigureAwait(false);
            return new TransientCategoryChannel(client, model);
        }

        public static async Task<IThreadChannel> ModifyThreadChannelAsync(this IRestClient client,
            Snowflake threadId, Action<ModifyThreadChannelActionProperties> action,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            Guard.IsNotNull(action);

            var model = await client.InternalModifyChannelAsync(threadId, action, options, cancellationToken).ConfigureAwait(false);
            return new TransientThreadChannel(client, model);
        }

        internal static Task<ChannelJsonModel> InternalModifyChannelAsync<T>(this IRestClient client,
            Snowflake channelId, Action<T> action,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
            where T : ModifyGuildChannelActionProperties
        {
            Guard.IsNotNull(action);

            // Can't use the new() generic constraint because the constructors are internal.
            // Can't use the generic CreateInstance either *because*.
            var properties = (T) Activator.CreateInstance(typeof(T), true);
            action(properties);
            var content = new ModifyChannelJsonRestRequestContent
            {
                Name = properties.Name,
                Position = properties.Position,
                PermissionOverwrites = Optional.Convert(properties.Overwrites, x => x.Select(x => x.ToModel()).ToArray())
            };

            switch (properties)
            {
                case ModifyNestableChannelActionProperties nestableProperties:
                {
                    content.ParentId = nestableProperties.CategoryId;

                    switch (nestableProperties)
                    {
                        case ModifyMessageGuildChannelActionProperties messageGuildChannelProperties:
                        {
                            content.RateLimitPerUser = Optional.Convert(messageGuildChannelProperties.Slowmode, x => (int) x.TotalSeconds);

                            switch (nestableProperties)
                            {
                                case ModifyTextChannelActionProperties textProperties:
                                {
                                    content.Topic = textProperties.Topic;
                                    content.Nsfw = textProperties.IsNsfw;
                                    content.DefaultAutoArchiveDuration = Optional.Convert(textProperties.DefaultAutomaticArchiveDuration, x => (int) x.TotalMinutes);
                                    break;
                                }
                                case ModifyThreadChannelActionProperties threadProperties:
                                {
                                    content.Archived = threadProperties.IsArchived;
                                    content.AutoArchiveDuration = Optional.Convert(threadProperties.AutomaticArchiveDuration, x => (int) x.TotalMinutes);
                                    content.Locked = threadProperties.IsLocked;
                                    content.Invitable = threadProperties.AllowsInvitation;
                                    break;
                                }
                            }

                            break;
                        }
                        case ModifyVoiceChannelActionProperties voiceProperties:
                        {
                            content.Bitrate = voiceProperties.Bitrate;
                            content.UserLimit = voiceProperties.MemberLimit;
                            content.RtcRegion = voiceProperties.Region;
                            content.VideoQualityMode = voiceProperties.VideoQualityMode;
                            break;
                        }
                    }

                    break;
                }
                case ModifyCategoryChannelActionProperties categoryProperties:
                {
                    // No extra properties for category channels.
                    break;
                }
                default:
                {
                    throw new ArgumentException($"Unknown channel action properties provided ({properties.GetType()}).");
                }
            }

            return client.ApiClient.ModifyChannelAsync(channelId, content, options, cancellationToken);
        }

        public static Task DeleteChannelAsync(this IRestClient client,
            Snowflake channelId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            return client.ApiClient.DeleteChannelAsync(channelId, options, cancellationToken);
        }

        public static IPagedEnumerable<IMessage> EnumerateMessages(this IRestClient client,
            Snowflake channelId, int limit, RetrievalDirection direction = RetrievalDirection.Before, Snowflake? startFromId = null,
            IRestRequestOptions options = null)
        {
            Guard.IsGreaterThanOrEqualTo(limit, 0);

            return PagedEnumerable.Create((state, cancellationToken) =>
            {
                var (client, channelId, limit, direction, startFromId, options) = state;
                return new FetchMessagesPagedEnumerator(client, channelId, limit, direction, startFromId, options, cancellationToken);
            }, (client, channelId, limit, direction, startFromId, options));
        }

        public static Task<IReadOnlyList<IMessage>> FetchMessagesAsync(this IRestClient client,
            Snowflake channelId, int limit = Discord.Limits.Rest.FetchMessagesPageSize, RetrievalDirection direction = RetrievalDirection.Before, Snowflake? startFromId = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            if (limit == 0)
                return Task.FromResult(ReadOnlyList<IMessage>.Empty);

            if (limit <= Discord.Limits.Rest.FetchMessagesPageSize)
                return client.InternalFetchMessagesAsync(channelId, limit, direction, startFromId, options, cancellationToken);

            var enumerable = client.EnumerateMessages(channelId, limit, direction, startFromId, options);
            return enumerable.FlattenAsync(cancellationToken);
        }

        internal static async Task<IReadOnlyList<IMessage>> InternalFetchMessagesAsync(this IRestClient client,
            Snowflake channelId, int limit, RetrievalDirection direction, Snowflake? startFromId,
            IRestRequestOptions options, CancellationToken cancellationToken)
        {
            var models = await client.ApiClient.FetchMessagesAsync(channelId, limit, direction, startFromId, options, cancellationToken).ConfigureAwait(false);
            return models.ToReadOnlyList(client, (x, client) => TransientMessage.Create(client, x));
        }

        public static async Task<IMessage> FetchMessageAsync(this IRestClient client,
            Snowflake channelId, Snowflake messageId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var model = await client.ApiClient.FetchMessageAsync(channelId, messageId, options, cancellationToken).ConfigureAwait(false);
                return TransientMessage.Create(client, model);
            }
            catch (RestApiException ex) when (ex.StatusCode == HttpResponseStatusCode.NotFound && ex.ErrorModel.Code == RestApiErrorCode.UnknownMessage)
            {
                return null;
            }
        }

        public static async Task<IUserMessage> SendMessageAsync(this IRestClient client,
            Snowflake channelId, LocalMessage message,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            Guard.IsNotNull(message);

            message.Validate();
            var messageContent = new CreateMessageJsonRestRequestContent
            {
                Content = Optional.FromNullable(message.Content),
                Tts = Optional.Conditional(message.IsTextToSpeech, true),
                Embeds = Optional.Conditional(message.Embeds.Count != 0, x => x.Select(x => x.ToModel()).ToArray(), message.Embeds),
                Flags = Optional.Conditional(message.Flags != 0, message.Flags),
                AllowedMentions = Optional.FromNullable(message.AllowedMentions.ToModel()),
                MessageReference = Optional.FromNullable(message.Reference.ToModel()),
                Nonce = Optional.FromNullable(message.Nonce),
                Components = Optional.Conditional(message.Components.Count != 0, x => x.Select(x => x.ToModel()).ToArray(), message.Components),
                StickerIds = Optional.Conditional(message.StickerIds.Count != 0, x => x.ToArray(), message.StickerIds)
            };

            Task<MessageJsonModel> task;
            if (message.Attachments.Count != 0)
            {
                // If there are attachments, we must send them via multipart HTTP content.
                // Our `messageContent` will be serialized into a "payload_json" form data field.
                var content = new MultipartJsonPayloadRestRequestContent<CreateMessageJsonRestRequestContent>(messageContent, message.Attachments);
                task = client.ApiClient.CreateMessageAsync(channelId, content, options, cancellationToken);
            }
            else
            {
                task = client.ApiClient.CreateMessageAsync(channelId, messageContent, options, cancellationToken);
            }

            var model = await task.ConfigureAwait(false);
            return new TransientUserMessage(client, model);
        }

        public static async Task<IUserMessage> CrosspostMessageAsync(this IRestClient client,
            Snowflake channelId, Snowflake messageId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var model = await client.ApiClient.CrosspostMessageAsync(channelId, messageId, options, cancellationToken).ConfigureAwait(false);
            return new TransientUserMessage(client, model);
        }

        public static Task AddReactionAsync(this IRestClient client,
            Snowflake channelId, Snowflake messageId, LocalEmoji emoji,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            Guard.IsNotNull(emoji);

            return client.ApiClient.AddReactionAsync(channelId, messageId, emoji.GetReactionFormat(), options, cancellationToken);
        }

        public static Task RemoveOwnReactionAsync(this IRestClient client,
            Snowflake channelId, Snowflake messageId, LocalEmoji emoji,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            Guard.IsNotNull(emoji);

            return client.ApiClient.RemoveOwnReactionAsync(channelId, messageId, emoji.GetReactionFormat(), options, cancellationToken);
        }

        public static Task RemoveReactionAsync(this IRestClient client,
            Snowflake channelId, Snowflake messageId, LocalEmoji emoji, Snowflake userId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            Guard.IsNotNull(emoji);

            if (client.ApiClient.Token is BotToken botToken && botToken.Id == userId)
                return client.ApiClient.RemoveOwnReactionAsync(channelId, messageId, emoji.GetReactionFormat(), options, cancellationToken);

            return client.ApiClient.RemoveUserReactionAsync(channelId, messageId, emoji.GetReactionFormat(), userId, options, cancellationToken);
        }

        public static IPagedEnumerable<IUser> EnumerateReactions(this IRestClient client,
            Snowflake channelId, Snowflake messageId, LocalEmoji emoji, int limit, Snowflake? startFromId = null,
            IRestRequestOptions options = null)
        {
            Guard.IsNotNull(emoji);
            Guard.IsGreaterThanOrEqualTo(limit, 0);

            return PagedEnumerable.Create((state, cancellationToken) =>
            {
                var (client, channelId, messageId, emoji, limit, startFromId, options) = state;
                return new FetchReactionsPagedEnumerator(client, channelId, messageId, emoji, limit, startFromId, options, cancellationToken);
            }, (client, channelId, messageId, emoji, limit, startFromId, options));
        }

        public static Task<IReadOnlyList<IUser>> FetchReactionsAsync(this IRestClient client,
            Snowflake channelId, Snowflake messageId, LocalEmoji emoji, int limit = Discord.Limits.Rest.FetchReactionsPageSize, Snowflake? startFromId = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            Guard.IsNotNull(emoji);

            if (limit == 0)
                return Task.FromResult(ReadOnlyList<IUser>.Empty);

            if (limit <= Discord.Limits.Rest.FetchReactionsPageSize)
                return client.InternalFetchReactionsAsync(channelId, messageId, emoji, limit, startFromId, options, cancellationToken);

            var enumerable = client.EnumerateReactions(channelId, messageId, emoji, limit, startFromId, options);
            return enumerable.FlattenAsync(cancellationToken);
        }

        internal static async Task<IReadOnlyList<IUser>> InternalFetchReactionsAsync(this IRestClient client,
            Snowflake channelId, Snowflake messageId, LocalEmoji emoji, int limit, Snowflake? startFromId,
            IRestRequestOptions options, CancellationToken cancellationToken)
        {
            var models = await client.ApiClient.FetchReactionsAsync(channelId, messageId, emoji.GetReactionFormat(), limit, startFromId, options, cancellationToken).ConfigureAwait(false);
            return models.ToReadOnlyList(client, (x, client) => new TransientUser(client, x));
        }

        public static Task ClearReactionsAsync(this IRestClient client,
            Snowflake channelId, Snowflake messageId, LocalEmoji emoji = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            if (emoji == null)
                return client.ApiClient.ClearReactionsAsync(channelId, messageId, options, cancellationToken);

            return client.ApiClient.ClearEmojiReactionsAsync(channelId, messageId, emoji.GetReactionFormat(), options, cancellationToken);
        }

        public static async Task<IUserMessage> ModifyMessageAsync(this IRestClient client,
            Snowflake channelId, Snowflake messageId, Action<ModifyMessageActionProperties> action,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            Guard.IsNotNull(action);

            var properties = new ModifyMessageActionProperties();
            action(properties);
            var messageContent = new ModifyMessageJsonRestRequestContent
            {
                Content = properties.Content,
                Embeds = Optional.Convert(properties.Embeds, models => models.Select(x =>
                {
                    x.Validate();
                    return x.ToModel();
                }).ToArray()),
                Flags = properties.Flags,
                AllowedMentions = Optional.Convert(properties.AllowedMentions, x => x.ToModel()),
                Attachments = Optional.Convert(properties.AttachmentIds, x => x.Select(x => new AttachmentJsonModel
                {
                    Id = x
                }).ToArray()),
                Components = Optional.Convert(properties.Components, models => models.Select(x => x.ToModel()).ToArray()),
                StickerIds = Optional.Convert(properties.StickerIds, x => x.ToArray())
            };

            Task<MessageJsonModel> task;
            LocalAttachment[] attachments;
            if (properties.Attachments.TryGetValue(out var attachmentsEnumerable) && (attachments = attachmentsEnumerable.GetArray()).Length != 0)
            {
                // If there are attachments, we must send them via multipart HTTP content.
                // Our `messageContent` will be serialized into a "payload_json" form data field.
                var content = new MultipartJsonPayloadRestRequestContent<ModifyMessageJsonRestRequestContent>(messageContent, attachments);
                task = client.ApiClient.ModifyMessageAsync(channelId, messageId, content, options, cancellationToken);
            }
            else
            {
                task = client.ApiClient.ModifyMessageAsync(channelId, messageId, messageContent, options, cancellationToken);
            }

            var model = await task.ConfigureAwait(false);
            return new TransientUserMessage(client, model);
        }

        public static Task DeleteMessageAsync(this IRestClient client,
            Snowflake channelId, Snowflake messageId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            return client.ApiClient.DeleteMessageAsync(channelId, messageId, options, cancellationToken);
        }

        public static IPagedEnumerable<Snowflake> EnumerateMessageDeletion(this IRestClient client,
            Snowflake channelId, IEnumerable<Snowflake> messageIds,
            IRestRequestOptions options = null)
        {
            Guard.IsNotNull(messageIds);

            return PagedEnumerable.Create((state, cancellationToken) =>
            {
                var (client, channelId, messageIds, options) = state;
                return new DeleteMessagesPagedEnumerator(client, channelId, messageIds, options, cancellationToken);
            }, (client, channelId, messageIds.ToArray(), options));
        }

        public static async Task DeleteMessagesAsync(this IRestClient client,
            Snowflake channelId, IEnumerable<Snowflake> messageIds,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            Guard.IsNotNull(messageIds);

            var messages = messageIds.ToArray();
            if (messages.Length == 0)
                return;

            if (messages.Length == 1)
            {
                await client.DeleteMessageAsync(channelId, messages[0], options, cancellationToken).ConfigureAwait(false);
                return;
            }

            if (messages.Length <= Discord.Limits.Rest.DeleteMessagesPageSize)
            {
                await client.InternalDeleteMessagesAsync(channelId, messages, options, cancellationToken).ConfigureAwait(false);
                return;
            }

            var enumerator = client.EnumerateMessageDeletion(channelId, messages, options).GetAsyncEnumerator(cancellationToken);
            await using (enumerator.ConfigureAwait(false))
            {
                // Exhaust the enumerator.
                while (await enumerator.MoveNextAsync().ConfigureAwait(false))
                { }
            }
        }

        internal static Task InternalDeleteMessagesAsync(this IRestClient client,
            Snowflake channelId, ArraySegment<Snowflake> messageIds,
            IRestRequestOptions options, CancellationToken cancellationToken)
        {
            var content = new DeleteMessagesJsonRestRequestContent
            {
                Messages = messageIds
            };

            return client.ApiClient.DeleteMessagesAsync(channelId, content, options, cancellationToken);
        }

        public static Task SetOverwriteAsync(this IRestClient client,
            Snowflake channelId, LocalOverwrite overwrite,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var content = new SetOverwriteJsonRestRequestContent
            {
                Type = overwrite.TargetType,
                Allow = overwrite.Permissions.Allowed.RawValue,
                Deny = overwrite.Permissions.Denied.RawValue
            };

            return client.ApiClient.SetOverwriteAsync(channelId, overwrite.TargetId, content, options, cancellationToken);
        }

        public static Task DeleteOverwriteAsync(this IRestClient client,
            Snowflake channelId, Snowflake targetId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            return client.ApiClient.DeleteOverwriteAsync(channelId, targetId, options, cancellationToken);
        }

        public static async Task<IReadOnlyList<IInvite>> FetchChannelInvitesAsync(this IRestClient client,
            Snowflake channelId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var models = await client.ApiClient.FetchChannelInvitesAsync(channelId, options, cancellationToken).ConfigureAwait(false);
            return models.ToReadOnlyList(client, (x, client) => TransientInvite.Create(client, x));
        }

        /// <summary>
        ///     Creates a channel invite.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="channelId"></param>
        /// <param name="maxAge"> The max age of the invite. Defaults to 86400 seconds (24 hours). Pass <see cref="Timeout.InfiniteTimeSpan"/> for an indefinite invite. </param>
        /// <param name="maxUses"></param>
        /// <param name="isTemporaryMembership"></param>
        /// <param name="isUnique"></param>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<IInvite> CreateInviteAsync(this IRestClient client,
            Snowflake channelId, TimeSpan maxAge = default, int maxUses = 0, bool isTemporaryMembership = false, bool isUnique = false,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var content = new CreateChannelInviteJsonRestRequestContent
            {
                MaxAge = maxAge != default
                    ? maxAge != Timeout.InfiniteTimeSpan
                        ? (int) maxAge.TotalSeconds
                        : 0
                    : 86400,
                MaxUses = maxUses,
                Temporary = isTemporaryMembership,
                Unique = isUnique
            };

            var model = await client.ApiClient.CreateChannelInviteAsync(channelId, content, options, cancellationToken).ConfigureAwait(false);
            return TransientInvite.Create(client, model);
        }

        public static async Task<IFollowedChannel> FollowNewsChannelAsync(this IRestClient client,
            Snowflake channelId, Snowflake targetChannelId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var content = new FollowNewsChannelJsonRestRequestContent
            {
                WebhookChannelId = targetChannelId
            };

            var model = await client.ApiClient.FollowNewsChannelAsync(channelId, content, options, cancellationToken).ConfigureAwait(false);
            return new TransientFollowedChannel(client, model);
        }

        public static Task TriggerTypingAsync(this IRestClient client,
            Snowflake channelId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            return client.ApiClient.TriggerTypingAsync(channelId, options, cancellationToken);
        }

        /// <summary>
        ///     Begins typing in the specified channel, i.e. calls <see cref="TriggerTypingAsync"/> at an interval
        ///     until the returned <see cref="IDisposable"/> is disposed.
        /// </summary>
        /// <param name="client"> The REST client. </param>
        /// <param name="channelId"> The ID of the channel to begin typing in. </param>
        /// <param name="options"> The request options. </param>
        /// <param name="cancellationToken"> The cancellation token to observe. </param>
        /// <returns>
        ///     A disposable object that stops the typing.
        /// </returns>
        public static IDisposable BeginTyping(this IRestClient client,
            Snowflake channelId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            return new TypingRepeater(client, channelId, options, cancellationToken);
        }

        public static async Task<IReadOnlyList<IUserMessage>> FetchPinnedMessagesAsync(this IRestClient client,
            Snowflake channelId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var models = await client.ApiClient.FetchPinnedMessagesAsync(channelId, options, cancellationToken).ConfigureAwait(false);
            return models.ToReadOnlyList(client, (x, client) => new TransientUserMessage(client, x));
        }

        public static Task PinMessageAsync(this IRestClient client,
            Snowflake channelId, Snowflake messageId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            return client.ApiClient.PinMessageAsync(channelId, messageId, options, cancellationToken);
        }

        public static Task UnpinMessageAsync(this IRestClient client,
            Snowflake channelId, Snowflake messageId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            return client.ApiClient.UnpinMessageAsync(channelId, messageId, options, cancellationToken);
        }

        public static async Task<IThreadChannel> CreatePublicThreadAsync(this IRestClient client,
            Snowflake channelId, string name, Snowflake? messageId = null, TimeSpan? automaticArchiveDuration = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var content = new CreateThreadJsonRestRequestContent
            {
                Name = name,
                AutoArchiveDuration = Optional.Conditional(automaticArchiveDuration != null, duration => (int) duration.Value.TotalMinutes, automaticArchiveDuration),
                Type = Optional.Conditional(messageId == null, ChannelType.PublicThread)
            };

            var model = await client.ApiClient.CreateThreadAsync(channelId, content, messageId, options, cancellationToken).ConfigureAwait(false);
            return new TransientThreadChannel(client, model);
        }

        public static async Task<IThreadChannel> CreatePrivateThreadAsync(this IRestClient client,
            Snowflake channelId, string name, TimeSpan? automaticArchiveDuration = null, bool? allowInvitation = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var content = new CreateThreadJsonRestRequestContent
            {
                Name = name,
                AutoArchiveDuration = Optional.Conditional(automaticArchiveDuration != null, duration => (int) duration.Value.TotalMinutes, automaticArchiveDuration),
                Type = ChannelType.PrivateThread,
                Invitable = Optional.FromNullable(allowInvitation)
            };

            var model = await client.ApiClient.CreateThreadAsync(channelId, content, null, options, cancellationToken).ConfigureAwait(false);
            return new TransientThreadChannel(client, model);
        }

        public static Task JoinThreadAsync(this IRestClient client,
            Snowflake threadId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            return client.ApiClient.JoinThreadAsync(threadId, options, cancellationToken);
        }

        public static Task AddThreadMemberAsync(this IRestClient client,
            Snowflake threadId, Snowflake memberId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            return client.ApiClient.AddThreadMemberAsync(threadId, memberId, options, cancellationToken);
        }

        public static Task LeaveThreadAsync(this IRestClient client,
            Snowflake threadId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            return client.ApiClient.LeaveThreadAsync(threadId, options, cancellationToken);
        }

        public static Task RemoveThreadMemberAsync(this IRestClient client,
            Snowflake threadId, Snowflake memberId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            return client.ApiClient.RemoveThreadMemberAsync(threadId, memberId, options, cancellationToken);
        }

        public static async Task<IThreadMember> FetchThreadMemberAsync(this IRestClient client,
            Snowflake threadId, Snowflake memberId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var model = await client.ApiClient.FetchThreadMemberAsync(threadId, memberId, options, cancellationToken).ConfigureAwait(false);
                return new TransientThreadMember(client, model);
            }
            catch (RestApiException ex) when (ex.StatusCode == HttpResponseStatusCode.NotFound && ex.IsError(RestApiErrorCode.UnknownMember))
            {
                return null;
            }
        }

        public static async Task<IReadOnlyList<IThreadMember>> FetchThreadMembersAsync(this IRestClient client,
            Snowflake threadId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var models = await client.ApiClient.FetchThreadMembersAsync(threadId, options, cancellationToken).ConfigureAwait(false);
            return models.ToReadOnlyList(client, (x, client) => new TransientThreadMember(client, x));
        }

        public static IPagedEnumerable<IThreadChannel> EnumeratePublicArchivedThreads(this IRestClient client,
            Snowflake channelId, int limit, DateTimeOffset? startFromDate = null,
            IRestRequestOptions options = null)
        {
            Guard.IsGreaterThanOrEqualTo(limit, 0);

            return PagedEnumerable.Create((state, cancellationToken) =>
            {
                var (client, channelId, limit, startFromDate, options) = state;
                return new FetchArchivedThreadsPagedEnumerator(client, channelId, limit, startFromDate, true, options, cancellationToken);
            }, (client, channelId, limit, startFromDate, options));
        }

        public static async Task<IReadOnlyList<IThreadChannel>> FetchPublicArchivedThreadsAsync(this IRestClient client,
            Snowflake channelId, int limit = Discord.Limits.Rest.FetchThreadsPageSize, DateTimeOffset? startFromDate = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            if (limit == 0)
                return ReadOnlyList<IThreadChannel>.Empty;

            if (limit <= Discord.Limits.Rest.FetchThreadsPageSize)
                return (await client.InternalFetchPublicArchivedThreadsAsync(channelId, limit, startFromDate, options, cancellationToken).ConfigureAwait(false)).Threads;

            var enumerable = client.EnumeratePublicArchivedThreads(channelId, limit, startFromDate, options);
            return await enumerable.FlattenAsync(cancellationToken).ConfigureAwait(false);
        }

        internal static async Task<(bool HasMore, IReadOnlyList<IThreadChannel> Threads)> InternalFetchPublicArchivedThreadsAsync(this IRestClient client,
            Snowflake channelId, int limit, DateTimeOffset? startFromDate,
            IRestRequestOptions options, CancellationToken cancellationToken)
        {
            var model = await client.ApiClient.FetchPublicArchivedThreadsAsync(channelId, limit, startFromDate, options, cancellationToken).ConfigureAwait(false);
            return CreateThreads(client, model);
        }

        public static IPagedEnumerable<IThreadChannel> EnumeratePrivateArchivedThreads(this IRestClient client,
            Snowflake channelId, int limit, DateTimeOffset? startFromDate = null,
            IRestRequestOptions options = null)
        {
            Guard.IsGreaterThanOrEqualTo(limit, 0);

            return PagedEnumerable.Create((state, cancellationToken) =>
            {
                var (client, channelId, limit, startFromDate, options) = state;
                return new FetchArchivedThreadsPagedEnumerator(client, channelId, limit, startFromDate, false, options, cancellationToken);
            }, (client, channelId, limit, startFromDate, options));
        }

        public static async Task<IReadOnlyList<IThreadChannel>> FetchPrivateArchivedThreadsAsync(this IRestClient client,
            Snowflake channelId, int limit = Discord.Limits.Rest.FetchThreadsPageSize, DateTimeOffset? startFromDate = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            if (limit == 0)
                return ReadOnlyList<IThreadChannel>.Empty;

            if (limit <= Discord.Limits.Rest.FetchThreadsPageSize)
                return (await client.InternalFetchPrivateArchivedThreadsAsync(channelId, limit, startFromDate, options, cancellationToken).ConfigureAwait(false)).Threads;

            var enumerable = client.EnumeratePrivateArchivedThreads(channelId, limit, startFromDate, options);
            return await enumerable.FlattenAsync(cancellationToken).ConfigureAwait(false);
        }

        internal static async Task<(bool HasMore, IReadOnlyList<IThreadChannel> Threads)> InternalFetchPrivateArchivedThreadsAsync(this IRestClient client,
            Snowflake channelId, int limit, DateTimeOffset? startFromDate,
            IRestRequestOptions options, CancellationToken cancellationToken)
        {
            var model = await client.ApiClient.FetchPrivateArchivedThreadsAsync(channelId, limit, startFromDate, options, cancellationToken).ConfigureAwait(false);
            return CreateThreads(client, model);
        }

        public static IPagedEnumerable<IThreadChannel> EnumerateJoinedPrivateArchivedThreads(this IRestClient client,
            Snowflake channelId, int limit, Snowflake? startFromId = null,
            IRestRequestOptions options = null)
        {
            Guard.IsGreaterThanOrEqualTo(limit, 0);

            return PagedEnumerable.Create((state, cancellationToken) =>
            {
                var (client, channelId, limit, startFromId, options) = state;
                return new FetchJoinedPrivateArchivedThreadsPagedEnumerator(client, channelId, limit, startFromId, options, cancellationToken);
            }, (client, channelId, limit, startFromId, options));
        }

        public static async Task<IReadOnlyList<IThreadChannel>> FetchJoinedPrivateArchivedThreadsAsync(this IRestClient client,
            Snowflake channelId, int limit = Discord.Limits.Rest.FetchThreadsPageSize, Snowflake? startFromId = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            if (limit == 0)
                return ReadOnlyList<IThreadChannel>.Empty;

            if (limit <= Discord.Limits.Rest.FetchThreadsPageSize)
                return (await client.InternalFetchJoinedPrivateArchivedThreadsAsync(channelId, limit, startFromId, options, cancellationToken).ConfigureAwait(false)).Threads;

            var enumerable = client.EnumerateJoinedPrivateArchivedThreads(channelId, limit, startFromId, options);
            return await enumerable.FlattenAsync(cancellationToken).ConfigureAwait(false);
        }

        internal static async Task<(bool HasMore, IReadOnlyList<IThreadChannel> Threads)> InternalFetchJoinedPrivateArchivedThreadsAsync(this IRestClient client,
            Snowflake channelId, int limit, Snowflake? startFromId,
            IRestRequestOptions options, CancellationToken cancellationToken)
        {
            var model = await client.ApiClient.FetchJoinedPrivateArchivedThreadsAsync(channelId, limit, startFromId, options, cancellationToken).ConfigureAwait(false);
            return CreateThreads(client, model);
        }

        private static (bool HasMore, IReadOnlyList<IThreadChannel> Threads) CreateThreads(IRestClient client,
            ThreadListJsonModel model)
        {
            static ChannelJsonModel MatchMemberToThread(ChannelJsonModel threadModel, ThreadMemberJsonModel[] memberModels)
            {
                var memberModel = Array.Find(memberModels, x => x.Id == threadModel.Id);
                if (memberModel != null)
                    threadModel.Member = memberModel;

                return threadModel;
            }

            return (model.HasMore, model.Threads.ToReadOnlyList((client, model.Members), (threadModel, tuple) =>
            {
                var (client, memberModels) = tuple;
                return new TransientThreadChannel(client, MatchMemberToThread(threadModel, memberModels));
            }));
        }
    }
}
