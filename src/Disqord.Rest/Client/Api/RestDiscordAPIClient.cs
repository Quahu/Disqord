using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Logging;
using Disqord.Models;
using Disqord.Serialization.Json;
using Disqord.Serialization.Json.Newtonsoft;

namespace Disqord.Rest
{
    internal sealed class RestDiscordApiClient : IDisposable
    {
        public const int API_VERSION = 7;

        public const string API_URL = "https://discordapp.com/api/v7/";

        private static readonly HttpMethod DELETE = HttpMethod.Delete;
        private static readonly HttpMethod GET = HttpMethod.Get;
        private static readonly HttpMethod PATCH = HttpMethod.Patch;
        private static readonly HttpMethod POST = HttpMethod.Post;
        private static readonly HttpMethod PUT = HttpMethod.Put;
        private static readonly TimeSpan _defaultTimeout = TimeSpan.FromSeconds(20);

        internal readonly HttpClient Http;

        internal readonly TokenType TokenType;

        internal readonly string Token;

        internal readonly ILogger Logger;

        internal readonly IJsonSerializer Serializer;

        private readonly RateLimiter _rateLimiter;

        public RestDiscordApiClient(TokenType tokenType, string token, ILogger logger, IJsonSerializer serializer)
        {
            TokenType = tokenType;
            Http = new HttpClient(new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
            })
            {
                BaseAddress = new Uri(API_URL, UriKind.Absolute),
                Timeout = Timeout.InfiniteTimeSpan
            };
            Http.DefaultRequestHeaders.Add("Accept-Encoding", "deflate, gzip");

            if (token != null)
            {
                Token = PrefixToken(tokenType, token);
                Http.DefaultRequestHeaders.Add("Authorization", Token);
            }

            Logger = logger ?? new DefaultLogger();
            Serializer = serializer ?? GetDefaultSerializer();
            _rateLimiter = RateLimiter.GetOrCreate(this);
        }

        private IJsonSerializer GetDefaultSerializer()
            => NewtonsoftJsonSerializer.Instance;

        internal static string PrefixToken(TokenType tokenType, string token)
            => tokenType switch
            {
                TokenType.Bearer => $"Bearer {token}",
                TokenType.Bot => $"Bot {token}",
                TokenType.User => token,
                _ => throw new ArgumentOutOfRangeException(nameof(tokenType), "Invalid token type."),
            };

        private async Task EnqueueRequestAsync(RestRequest request)
        {
            request.Initialise(Serializer);
            await _rateLimiter.EnqueueRequestAsync(request).ConfigureAwait(false);
        }

        internal async Task<RateLimit> HandleRequestAsync(RestRequest request)
        {
            Log(LogMessageSeverity.Debug, $"Handling {request}.");
            HttpResponseMessage response;
            var options = request.Options;
            using (var cts = new CancellationTokenSource(options != null && options.Timeout != default ? options.Timeout : _defaultTimeout))
            using (var linkedCts = options != null ? CancellationTokenSource.CreateLinkedTokenSource(cts.Token, options.CancellationToken) : null)
            {
                var ticks = Environment.TickCount;
                response = await Http.SendAsync(request.HttpMessage, HttpCompletionOption.ResponseHeadersRead, linkedCts?.Token ?? cts.Token).ConfigureAwait(false);
                var ms = Environment.TickCount - ticks;
                Log(LogMessageSeverity.Debug, $"Handling {request}; completed after {ms}ms.");
            }

            var rateLimit = new RateLimit(response.Headers);
            if (Library.Debug.DumpRateLimits)
                Console.WriteLine(rateLimit);

            if (!response.IsSuccessStatusCode)
            {
                switch ((int) response.StatusCode)
                {
                    case 429: // TODO
                    {
                        await _rateLimiter.HandleRateLimitedAsync(rateLimit).ConfigureAwait(false);

                        if (rateLimit.IsGlobal)
                            await SetExceptionAsync(response, request).ConfigureAwait(false);

                        break;
                    }

                    default:
                        await SetExceptionAsync(response, request).ConfigureAwait(false);
                        break;
                }
            }
            else
            {
                request.SetResult(response);
            }

            return rateLimit;
        }

        private async Task SetExceptionAsync(HttpResponseMessage response, RestRequest request)
        {
            using (var jsonStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
            {
                var error = Serializer.Deserialize<JsonErrorModel>(jsonStream);
                request.SetException(new DiscordHttpException(response.StatusCode, (int?) error.Code, error.Message ?? response.ReasonPhrase));
            }
        }

        internal async Task SendRequestAsync(RestRequest request)
        {
            await EnqueueRequestAsync(request).ConfigureAwait(false);
            _ = await request.CompleteAsync().ConfigureAwait(false);
        }

        internal async Task<T> SendRequestAsync<T>(RestRequest request) where T : class
        {
            await EnqueueRequestAsync(request).ConfigureAwait(false);
            var response = await request.CompleteAsync().ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.NoContent)
                return null;

            using (var jsonStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
            {
                return await Serializer.DeserializeAsync<T>(jsonStream).ConfigureAwait(false);
            }
        }

        // Audit Log
        public Task<AuditLogModel> GetGuildAuditLogAsync(ulong guildId, int limit, ulong? userId, AuditLogType? type, ulong? before, RestRequestOptions options)
        {
            var parameters = new Dictionary<string, object>
            {
                ["limit"] = limit
            };

            if (userId != null)
                parameters["user_id"] = userId;

            if (type != null)
                parameters["action_type"] = (int) type;

            if (before != null)
                parameters["before"] = before;

            return SendRequestAsync<AuditLogModel>(new RestRequest(GET, $"guilds/{guildId:guild_id}/audit-logs", parameters, options));
        }

        // Channel
        public Task<ChannelModel> GetChannelAsync(ulong channelId, RestRequestOptions options)
            => SendRequestAsync<ChannelModel>(new RestRequest(GET, $"channels/{channelId:channel_id}", options));

        public Task<ChannelModel> ModifyChannelAsync(ulong channelId, ModifyChannelProperties properties, RestRequestOptions options)
        {
            var requestContent = new ModifyChannelContent
            {
                Name = properties.Name,
            };

            if (properties is ModifyGuildChannelProperties guildProperties)
            {
                requestContent.Position = guildProperties.Position;
                requestContent.PermissionOverwrites = guildProperties.Overwrites.HasValue
                    ? guildProperties.Overwrites.Value.Select(x => x.ToModel()).ToArray()
                    : Optional<IReadOnlyList<OverwriteModel>>.Empty;

                if (guildProperties is ModifyNestedChannelProperties nestedProperties)
                {
                    requestContent.ParentId = nestedProperties.CategoryId.HasValue
                        ? nestedProperties.CategoryId.Value.RawValue
                        : Optional<ulong>.Empty;

                    if (nestedProperties is ModifyTextChannelProperties textProperties)
                    {
                        requestContent.Topic = textProperties.Topic;
                        requestContent.Nsfw = textProperties.IsNsfw;
                        requestContent.RateLimitPerUser = textProperties.Slowmode;
                    }
                    else if (nestedProperties is ModifyVoiceChannelProperties voiceProperties)
                    {
                        requestContent.Bitrate = voiceProperties.Bitrate;
                        requestContent.UserLimit = voiceProperties.UserLimit;
                    }
                    else
                    {
                        Log(LogMessageSeverity.Error, $"Unknown nested channel properties provided to modify. ({properties.GetType()})");
                    }
                }
                else if (guildProperties is ModifyCategoryChannelProperties categoryProperties)
                {
                    // No extra properties for category channels.
                }
                else
                {
                    Log(LogMessageSeverity.Error, $"Unknown guild channel properties provided to modify. ({properties.GetType()})");
                }
            }
            else if (properties is ModifyGroupChannelProperties groupProperties)
            {
                requestContent.Icon = groupProperties.Icon;
            }
            else
            {
                Log(LogMessageSeverity.Error, $"Unknown channel properties provided to modify. ({properties.GetType()})");
            }

            return SendRequestAsync<ChannelModel>(new RestRequest(PATCH, $"channels/{channelId:channel_id}", requestContent, options));
        }

        public Task DeleteOrCloseChannelAsync(ulong channelId, RestRequestOptions options)
            => SendRequestAsync(new RestRequest(DELETE, $"channels/{channelId}", options));

        public Task<MessageModel[]> GetChannelMessagesAsync(ulong channelId, int limit, RetrievalDirection? direction, ulong? snowflake, RestRequestOptions options)
        {
            if (limit < 1 || limit > 100)
                throw new ArgumentOutOfRangeException(nameof(limit), "Message history limit must be a positive number not larger than 100.");

            var parameters = new Dictionary<string, object>
            {
                ["limit"] = limit
            };

            if (direction != null)
            {
                switch (direction.Value)
                {
                    case RetrievalDirection.Around:
                    {
                        parameters["around"] = snowflake ?? throw new ArgumentNullException(nameof(snowflake), "The snowflake to get messages around must not be null.");
                        break;
                    }

                    case RetrievalDirection.Before:
                    {
                        if (snowflake != null)
                            parameters["before"] = snowflake;

                        break;
                    }

                    case RetrievalDirection.After:
                    {
                        parameters["after"] = snowflake ?? throw new ArgumentNullException(nameof(snowflake), "The snowflake to get messages after must not be null.");
                        break;
                    }

                    default:
                        throw new ArgumentOutOfRangeException(nameof(direction), "Invalid message history direction.");
                }
            }

            return SendRequestAsync<MessageModel[]>(new RestRequest(GET, $"channels/{channelId:channel_id}/messages", parameters, options));
        }

        public Task<MessageModel> GetChannelMessageAsync(ulong channelId, ulong messageId, RestRequestOptions options)
            => SendRequestAsync<MessageModel>(new RestRequest(GET, $"channels/{channelId:channel_id}/messages/{messageId}", options));

        public Task<MessageModel> CreateMessageAsync(ulong channelId, string content, bool isTTS, LocalEmbed embed, RestRequestOptions options)
        {
            var requestContent = new CreateMessageContent
            {
                Content = content,
                Tts = isTTS,
                Embed = embed.ToModel()
            };
            return SendRequestAsync<MessageModel>(new RestRequest(POST, $"channels/{channelId:channel_id}/messages", requestContent, options));
        }

        public Task<MessageModel> CreateMessageAsync(ulong channelId, LocalAttachment attachment, string content, bool isTTS, LocalEmbed embed, RestRequestOptions options)
        {
            var requestContent = new MultipartRequestContent<CreateMessageContent>
            {
                Content = new CreateMessageContent
                {
                    Content = content,
                    Tts = isTTS,
                    Embed = embed.ToModel()
                },
                Attachment = attachment
            };
            return SendRequestAsync<MessageModel>(new RestRequest(POST, $"channels/{channelId:channel_id}/messages", requestContent, options));
        }

        public Task<MessageModel> CreateMessageAsync(ulong channelId, IEnumerable<LocalAttachment> attachments, string content, bool isTTS, LocalEmbed embed, RestRequestOptions options)
        {
            var requestContent = new MultipartRequestContent<CreateMessageContent>
            {
                Content = new CreateMessageContent
                {
                    Content = content,
                    Tts = isTTS,
                    Embed = embed.ToModel()
                },
                Attachments = attachments.ToArray()
            };
            return SendRequestAsync<MessageModel>(new RestRequest(POST, $"channels/{channelId:channel_id}/messages", requestContent, options));
        }

        public Task CreateReactionAsync(ulong channelId, ulong messageId, string emoji, RestRequestOptions options)
            => SendRequestAsync(new RestRequest(PUT, $"channels/{channelId:channel_id}/messages/{messageId}/reactions/{emoji}/@me", options));

        public Task DeleteOwnReactionAsync(ulong channelId, ulong messageId, string emoji, RestRequestOptions options)
            => SendRequestAsync(new RestRequest(DELETE, $"channels/{channelId:channel_id}/messages/{messageId}/reactions/{emoji}/@me", options));

        public Task DeleteUserReactionAsync(ulong channelId, ulong messageId, ulong userId, string emoji, RestRequestOptions options)
            => SendRequestAsync(new RestRequest(DELETE, $"channels/{channelId:channel_id}/messages/{messageId}/reactions/{emoji}/{userId}", options));

        public Task<UserModel[]> GetReactionsAsync(ulong channelId, ulong messageId, string emoji, int limit, RetrievalDirection? direction, ulong? snowflake, RestRequestOptions options)
        {
            if (limit < 1 || limit > 100)
                throw new ArgumentOutOfRangeException(nameof(limit), "Reaction users limit must be a positive number not larger than 100.");

            var parameters = new Dictionary<string, object>
            {
                ["limit"] = limit
            };

            if (direction != null)
            {
                switch (direction.Value)
                {
                    case RetrievalDirection.Around:
                        throw new NotSupportedException("Reaction users does not support Direction.Around.");

                    case RetrievalDirection.Before:
                    {
                        if (snowflake != null)
                            parameters["before"] = snowflake;

                        break;
                    }

                    case RetrievalDirection.After:
                    {
                        parameters["after"] = snowflake ?? throw new ArgumentNullException(nameof(snowflake), "The snowflake to get reaction users after must not be null.");
                        break;
                    }

                    default:
                        throw new ArgumentOutOfRangeException(nameof(direction), "Invalid reaction users direction.");
                }
            }

            return SendRequestAsync<UserModel[]>(new RestRequest(GET, $"channels/{channelId:channel_id}/messages/{messageId}/reactions/{emoji}", parameters, options));
        }

        public Task DeleteAllReactionsAsync(ulong channelId, ulong messageId, RestRequestOptions options)
            => SendRequestAsync(new RestRequest(DELETE, $"channels/{channelId:channel_id}/messages/{messageId}/reactions", options));

        public Task<MessageModel> EditMessageAsync(ulong channelId, ulong messageId, ModifyMessageProperties properties, RestRequestOptions options)
        {
            var requestContent = new EditMessageContent
            {
                Content = properties.Content,
                Embed = properties.Embed.HasValue ? properties.Embed.Value.ToModel() : Optional<EmbedModel>.Empty
            };
            return SendRequestAsync<MessageModel>(new RestRequest(PATCH, $"channels/{channelId:channel_id}/messages/{messageId}", requestContent, options));
        }

        public Task DeleteMessageAsync(ulong channelId, ulong messageId, RestRequestOptions options)
            => SendRequestAsync(new RestRequest(DELETE, $"channels/{channelId:channel_id}/messages/{messageId}", options)
            {
                BucketsMethod = true
            });

        public Task BulkDeleteMessagesAsync(ulong channelId, IEnumerable<ulong> messageIds, RestRequestOptions options)
        {
            var requestContent = new BulkDeleteMessagesContent
            {
                Messages = messageIds.ToArray()
            };
            return SendRequestAsync(new RestRequest(POST, $"channels/{channelId:channel_id}/messages/bulk-delete", requestContent, options));
        }

        public Task EditChannelPermissionsAsync(ulong channelId, LocalOverwrite overwrite, RestRequestOptions options)
        {
            var requestContent = new EditChannelPermissionsContent
            {
                Allow = overwrite.Permissions.Allowed,
                Deny = overwrite.Permissions.Denied,
                Type = overwrite.TargetType
            };
            return SendRequestAsync(new RestRequest(PUT, $"channels/{channelId:channel_id}/permissions/{overwrite.TargetId}", requestContent, options));
        }

        public Task<InviteModel[]> GetChannelInvitesAsync(ulong channelId, RestRequestOptions options)
            => SendRequestAsync<InviteModel[]>(new RestRequest(GET, $"channels/{channelId:channel_id}/invites", options));

        public Task<InviteModel> CreateInviteAsync(ulong channelId, int maxAge, int maxUses, bool isTemporaryMembership, bool isUnique, RestRequestOptions options)
        {
            if (maxAge < 0)
                throw new ArgumentOutOfRangeException(nameof(maxAge));

            if (maxUses < 0)
                throw new ArgumentOutOfRangeException(nameof(maxUses));

            var requestContent = new CreateInviteContent
            {
                MaxAge = maxAge,
                MaxUses = maxUses,
                Temporary = isTemporaryMembership,
                Unique = isUnique
            };
            return SendRequestAsync<InviteModel>(new RestRequest(POST, $"channels/{channelId:channel_id}/invites", requestContent, options));
        }

        public Task DeleteChannelPermissionAsync(ulong channelId, ulong targetId, RestRequestOptions options)
            => SendRequestAsync(new RestRequest(DELETE, $"channels/{channelId:channel_id}/permissions/{targetId}", options));

        public Task TriggerTypingIndicatorAsync(ulong channelId, RestRequestOptions options)
            => SendRequestAsync(new RestRequest(POST, $"channels/{channelId:channel_id}/typing", options));

        public Task<MessageModel[]> GetPinnedMessagesAsync(ulong channelId, RestRequestOptions options)
            => SendRequestAsync<MessageModel[]>(new RestRequest(GET, $"channels/{channelId:channel_id}/pins", options));

        public Task AddPinnedMessageAsync(ulong channelId, ulong messageId, RestRequestOptions options)
            => SendRequestAsync(new RestRequest(PUT, $"channels/{channelId:channel_id}/pins/{messageId}", options));

        public Task DeletePinnedMessageAsync(ulong channelId, ulong messageId, RestRequestOptions options)
            => SendRequestAsync(new RestRequest(DELETE, $"channels/{channelId:channel_id}/pins/{messageId}", options));

        public Task GroupDmRecipientAddAsync(ulong channelId, ulong userId, string nick, string accessToken, RestRequestOptions options)
        {
            var requestContent = new AddGroupRecipientContent
            {
                AccessToken = accessToken,
                Nick = nick
            };
            return SendRequestAsync(new RestRequest(PUT, $"channels/{channelId:channel_id}/recipients/{userId}", requestContent, options));
        }

        public Task GroupDmRecipientRemoveAsync(ulong channelId, ulong userId, RestRequestOptions options)
            => SendRequestAsync(new RestRequest(DELETE, $"channels/{channelId:channel_id}/recipients/{userId}", options));

        // Emoji
        public Task<EmojiModel[]> ListGuildEmojisAsync(ulong guildId, RestRequestOptions options)
            => SendRequestAsync<EmojiModel[]>(new RestRequest(GET, $"guilds/{guildId:guild_id}/emojis", options));

        public Task<EmojiModel> GetGuildEmojiAsync(ulong guildId, ulong emojiId, RestRequestOptions options)
            => SendRequestAsync<EmojiModel>(new RestRequest(GET, $"guilds/{guildId:guild_id}/emojis/{emojiId}", options));

        public Task<EmojiModel> CreateGuildEmojiAsync(ulong guildId, Stream image, string name, IEnumerable<ulong> roleIds, RestRequestOptions options)
        {
            var requestContent = new CreateGuildEmojiContent
            {
                Image = image,
                Name = name,
                RoleIds = roleIds?.ToArray()
            };
            return SendRequestAsync<EmojiModel>(new RestRequest(POST, $"guilds/{guildId:guild_id}/emojis", requestContent, options));
        }

        public Task<EmojiModel> ModifyGuildEmojiAsync(ulong guildId, ulong emojiId, ModifyGuildEmojiProperties properties, RestRequestOptions options)
        {
            var requestContent = new ModifyGuildEmojiContent
            {
                Name = properties.Name,
                RoleIds = properties.RoleIds.HasValue
                    ? properties.RoleIds.Value.Select(x => x.RawValue).ToArray()
                    : Optional<IReadOnlyList<ulong>>.Empty
            };
            return SendRequestAsync<EmojiModel>(new RestRequest(PATCH, $"guilds/{guildId:guild_id}/emojis/{emojiId}", requestContent, options));
        }

        public Task DeleteGuildEmojiAsync(ulong guildId, ulong emojiId, RestRequestOptions options)
            => SendRequestAsync(new RestRequest(DELETE, $"guilds/{guildId:guild_id}/emojis/{emojiId}", options));

        // Guild
        public Task<GuildModel> CreateGuildAsync(
            string name, string voiceRegionId, Stream icon, VerificationLevel verificationLevel,
            DefaultNotificationLevel defaultNotificationLevel, ContentFilterLevel contentFilterLevel,
            RestRequestOptions options)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (name.Length < 2 || name.Length > 100)
                throw new ArgumentOutOfRangeException(nameof(name));

            var requestContent = new CreateGuildContent
            {
                Name = name,
                Region = voiceRegionId,
                Icon = icon,
                VerificationLevel = verificationLevel,
                DefaultNotificationLevel = defaultNotificationLevel,
                ContentFilterLevel = contentFilterLevel
            };
            return SendRequestAsync<GuildModel>(new RestRequest(POST, $"guilds", requestContent, options));
        }

        public Task<GuildModel> GetGuildAsync(ulong guildId, RestRequestOptions options)
            => SendRequestAsync<GuildModel>(new RestRequest(GET, $"guilds/{guildId:guild_id}", options));

        public Task<GuildModel> ModifyGuildAsync(ulong guildId, ModifyGuildProperties properties, RestRequestOptions options)
        {
            var requestContent = new ModifyGuildContent
            {
                Name = properties.Name,
                Region = properties.VoiceRegionId,
                VerificationLevel = properties.VerificationLevel,
                DefaultNotificationLevel = properties.DefaultNotificationLevel,
                ContentFilterLevel = properties.ContentFilterLevel,
                AfkChannelId = properties.AfkChannelId.HasValue
                   ? properties.AfkChannelId.Value.RawValue
                   : Optional<ulong>.Empty,
                AfkTimeout = properties.AfkTimeout,
                Icon = properties.Icon,
                OwnerId = properties.OwnerId.HasValue
                    ? properties.OwnerId.Value.RawValue
                    : Optional<ulong>.Empty,
                Splash = properties.Splash,
                SystemChannelId = properties.SystemChannelId.HasValue
                    ? properties.SystemChannelId.Value.RawValue
                    : Optional<ulong>.Empty,
                Banner = properties.Banner
            };
            return SendRequestAsync<GuildModel>(new RestRequest(PATCH, $"guilds/{guildId:guild_id}", requestContent, options));
        }

        public Task DeleteGuildAsync(ulong guildId, RestRequestOptions options)
            => SendRequestAsync(new RestRequest(DELETE, $"guilds/{guildId:guild_id}", options));

        public Task<ChannelModel[]> GetGuildChannelsAsync(ulong guildId, RestRequestOptions options)
            => SendRequestAsync<ChannelModel[]>(new RestRequest(GET, $"guilds/{guildId:guild_id}/channels", options));

        public Task<ChannelModel> CreateGuildChannelAsync(ulong guildId, string name, CreateGuildChannelProperties properties, RestRequestOptions options)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (name.Length < 2 || name.Length > 100)
                throw new ArgumentOutOfRangeException(nameof(name), $"The name must be between 2 and 100 characters long.");

            var requestContent = new CreateGuildChannelContent
            {
                Name = name,
                PermissionOvewrites = properties.Overwrites.HasValue
                    ? properties.Overwrites.Value.Select(x => x.ToModel()).ToArray()
                    : Optional<IReadOnlyList<OverwriteModel>>.Empty
            };

            if (properties is CreateNestedChannelProperties nestedProperties)
            {
                requestContent.ParentId = nestedProperties.ParentId.HasValue
                    ? nestedProperties.ParentId.Value.RawValue
                    : Optional<ulong>.Empty;

                if (properties is CreateTextChannelProperties textProperties)
                {
                    if (textProperties.Topic.HasValue && textProperties.Topic.Value != null && textProperties.Topic.Value.Length > 1024)
                        throw new ArgumentOutOfRangeException("Topic");

                    requestContent.Type = ChannelType.Text;
                    requestContent.Topic = textProperties.Topic;
                    requestContent.RateLimitPerUser = textProperties.Slowmode;
                    requestContent.Nsfw = textProperties.IsNsfw;
                }
                else if (properties is CreateVoiceChannelProperties voiceProperties)
                {
                    requestContent.Type = ChannelType.Voice;
                    requestContent.Bitrate = voiceProperties.Bitrate;
                    requestContent.UserLimit = voiceProperties.UserLimit;
                }
                else
                {
                    Log(LogMessageSeverity.Error, $"Unknown nested channel properties provided to modify ({properties.GetType()}).");
                }
            }
            else if (properties is CreateCategoryChannelProperties categoryProperties)
            {
                requestContent.Type = ChannelType.Category;
                // No extra properties for category channels.
            }
            else
            {
                Log(LogMessageSeverity.Error, $"Unknown channel properties provided to modify ({properties.GetType()}).");
            }

            return SendRequestAsync<ChannelModel>(new RestRequest(POST, $"guilds/{guildId:guild_id}/channels", requestContent, options));
        }

        public Task ModifyGuildChannelPositionsAsync(ulong guildId, IReadOnlyDictionary<Snowflake, int> channelPositions, RestRequestOptions options)
        {
            var positions = channelPositions.Select(x => new ModifyPositionsContent
            {
                Id = x.Key.RawValue,
                Position = x.Value
            });
            var requestContent = new RawJsonContent(Serializer.Serialize(positions));
            return SendRequestAsync(new RestRequest(PATCH, $"guilds/{guildId:guild_id}/channels", requestContent, options));
        }

        public Task<MemberModel> GetMemberAsync(ulong guildId, ulong userId, RestRequestOptions options)
            => SendRequestAsync<MemberModel>(new RestRequest(GET, $"guilds/{guildId:guild_id}/members/{userId}", options));

        public Task<MemberModel[]> ListGuildMembersAsync(ulong guildId, int limit, ulong after, RestRequestOptions options)
        {
            if (limit < 1 || limit > 1000)
                throw new ArgumentOutOfRangeException(nameof(limit), "Limit must be a positive integer not higher than 1000.");

            var parameters = new Dictionary<string, object>
            {
                ["limit"] = limit,
                ["after"] = after
            };
            return SendRequestAsync<MemberModel[]>(new RestRequest(GET, $"guilds/{guildId:guild_id}/members", parameters, options));
        }

        public Task ModifyCurrentUserNickAsync(ulong guildId, string nick, RestRequestOptions options)
        {
            var requestContent = new ModifyCurrentUserNickContent
            {
                Nick = nick
            };
            return SendRequestAsync(new RestRequest(PATCH, $"guilds/{guildId:guild_id}/members/@me/nick", requestContent, options));
        }

        public Task ModifyGuildMemberAsync(ulong guildId, ulong userId, ModifyMemberProperties properties, RestRequestOptions options)
        {
            var requestContent = new ModifyGuildMemberContent
            {
                Nick = properties.Nick.HasValue && properties.Nick.Value == null
                    ? ""
                    : properties.Nick,
                RoleIds = properties.RoleIds.HasValue
                    ? properties.RoleIds.Value.Select(x => x.RawValue).ToArray()
                    : Optional<IReadOnlyList<ulong>>.Empty,
                Mute = properties.Mute,
                Deaf = properties.Deaf,
                VoiceChannelId = properties.VoiceChannelId.HasValue
                    ? properties.VoiceChannelId.Value != null
                        ? properties.VoiceChannelId.Value.Value.RawValue
                        : (ulong?) null
                    : Optional<ulong?>.Empty
            };
            return SendRequestAsync(new RestRequest(PATCH, $"guilds/{guildId:guild_id}/members/{userId}", requestContent, options));
        }

        public Task AddGuildMemberRoleAsync(ulong guildId, ulong userId, ulong roleId, RestRequestOptions options)
            => SendRequestAsync(new RestRequest(PUT, $"guilds/{guildId:guild_id}/members/{userId}/roles/{roleId}", options));

        public Task DeleteGuildMemberRoleAsync(ulong guildId, ulong userId, ulong roleId, RestRequestOptions options)
            => SendRequestAsync(new RestRequest(DELETE, $"guilds/{guildId:guild_id}/members/{userId}/roles/{roleId}", options));

        public Task RemoveMemberAsync(ulong guildId, ulong userId, RestRequestOptions options)
            => SendRequestAsync(new RestRequest(DELETE, $"guilds/{guildId:guild_id}/members/{userId}", options));

        public Task<BanModel[]> GetGuildBansAsync(ulong guildId, RestRequestOptions options)
            => SendRequestAsync<BanModel[]>(new RestRequest(GET, $"guilds/{guildId:guild_id}/bans", options));

        public Task<BanModel> GetGuildBanAsync(ulong guildId, ulong userId, RestRequestOptions options)
            => SendRequestAsync<BanModel>(new RestRequest(GET, $"guilds/{guildId:guild_id}/bans/{userId}", options));

        public Task CreateGuildBanAsync(ulong guildId, ulong userId, string reason, int? deleteMessageDays, RestRequestOptions options)
        {
            Dictionary<string, object> parameters = null;
            if (reason != null || deleteMessageDays != null)
            {
                parameters = new Dictionary<string, object>();
                if (reason != null)
                    parameters["reason"] = reason;

                if (deleteMessageDays != null)
                    parameters["delete-message-days"] = deleteMessageDays.Value;
            }
            return SendRequestAsync(new RestRequest(PUT, $"guilds/{guildId:guild_id}/bans/{userId}", parameters, options));
        }

        public Task RemoveGuildBanAsync(ulong guildId, ulong userId, RestRequestOptions options)
            => SendRequestAsync(new RestRequest(DELETE, $"guilds/{guildId:guild_id}/bans/{userId}", options));

        public Task<RoleModel[]> GetGuildRolesAsync(ulong guildId, RestRequestOptions options)
            => SendRequestAsync<RoleModel[]>(new RestRequest(GET, $"guilds/{guildId:guild_id}/roles", options));

        public Task<RoleModel> CreateGuildRoleAsync(ulong guildId, CreateRoleProperties properties, RestRequestOptions options)
        {
            var requestContent = new CreateGuildRoleContent
            {
                Name = properties.Name,
                Permissions = properties.Permissions.HasValue
                    ? properties.Permissions.Value.RawValue
                    : Optional<ulong>.Empty,
                Color = properties.Color.HasValue
                    ? properties.Color.Value.RawValue
                    : Optional<int>.Empty,
                Hoist = properties.IsHoisted,
                Mentionable = properties.IsMentionable
            };
            return SendRequestAsync<RoleModel>(new RestRequest(POST, $"guilds/{guildId:guild_id}/roles", requestContent, options));
        }

        public Task<RoleModel[]> ModifyGuildRolePositionsAsync(ulong guildId, IReadOnlyDictionary<Snowflake, int> rolePositions, RestRequestOptions options)
        {
            var positions = rolePositions.Select(x => new ModifyPositionsContent
            {
                Id = x.Key.RawValue,
                Position = x.Value
            });
            var requestContent = new RawJsonContent(Serializer.Serialize(positions));
            return SendRequestAsync<RoleModel[]>(new RestRequest(PATCH, $"guilds/{guildId:guild_id}/roles", requestContent, options));
        }

        public Task<RoleModel> ModifyGuildRoleAsync(ulong guildId, ulong roleId, ModifyRoleProperties properties, RestRequestOptions options)
        {
            var requestContent = new ModifyGuildRoleContent
            {
                Name = properties.Name,
                Permissions = properties.Permissions.HasValue ? properties.Permissions.Value.RawValue : Optional<ulong>.Empty,
                Color = properties.Color.HasValue
                    ? properties.Color.Value?.RawValue ?? 0
                    : Optional<int>.Empty,
                Hoist = properties.IsHoisted,
                Mentionable = properties.IsMentionable
            };
            return SendRequestAsync<RoleModel>(new RestRequest(PATCH, $"guilds/{guildId:guild_id}/roles/{roleId}", requestContent, options));
        }

        public Task DeleteGuildRoleAsync(ulong guildId, ulong roleId, RestRequestOptions options)
            => SendRequestAsync(new RestRequest(DELETE, $"guilds/{guildId:guild_id}/roles/{roleId}", options));

        public async Task<int> GetGuildPruneCountAsync(ulong guildId, int days, RestRequestOptions options)
        {
            if (days <= 0)
                throw new ArgumentOutOfRangeException(nameof(days));

            var parameters = new Dictionary<string, object>
            {
                ["days"] = days
            };
            var model = await SendRequestAsync<PruneModel>(new RestRequest(GET, $"guilds/{guildId:guild_id}/prune", parameters, options)).ConfigureAwait(false);
            return model.Pruned.Value;
        }

        public async Task<int?> BeginGuildPruneAsync(ulong guildId, int days, bool computePruneCount, RestRequestOptions options)
        {
            if (days <= 0)
                throw new ArgumentOutOfRangeException(nameof(days));

            var parameters = new Dictionary<string, object>
            {
                ["days"] = days,
                ["compute_prune_count"] = computePruneCount
            };
            var model = await SendRequestAsync<PruneModel>(new RestRequest(POST, $"guilds/{guildId:guild_id}/prune", parameters, options)).ConfigureAwait(false);
            return model.Pruned;
        }

        public Task<VoiceRegionModel[]> GetGuildVoiceRegionsAsync(ulong guildId, RestRequestOptions options)
            => SendRequestAsync<VoiceRegionModel[]>(new RestRequest(GET, $"guilds/{guildId:guild_id}/regions", options));

        public Task<InviteModel[]> GetGuildInvitesAsync(ulong guildId, RestRequestOptions options)
            => SendRequestAsync<InviteModel[]>(new RestRequest(GET, $"guilds/{guildId:guild_id}/invites", options));

        public Task<IntegrationModel[]> GetGuildIntegrationsAsync(ulong guildId, RestRequestOptions options)
            => SendRequestAsync<IntegrationModel[]>(new RestRequest(GET, $"guilds/{guildId:guild_id}/integrations", options));

        public Task CreateGuildIntegrationsAsync(ulong guildId, string type, string id, RestRequestOptions options)
        {
            var requestContent = new CreateGuildIntegrationContent
            {
                Type = type,
                Id = id
            };
            return SendRequestAsync(new RestRequest(POST, $"guilds/{guildId:guild_id}/integrations", requestContent, options));
        }

        public Task<WidgetModel> GetGuildEmbedAsync(ulong guildId, RestRequestOptions options)
            => SendRequestAsync<WidgetModel>(new RestRequest(GET, $"guilds/{guildId:guild_id}/embed", options));

        public Task<WidgetModel> ModifyGuildEmbedAsync(ulong guildId, ModifyWidgetProperties properties, RestRequestOptions options)
        {
            var requestContent = new ModifyGuildEmbedContent
            {
                Enabled = properties.IsEnabled,
                ChannelId = properties.ChannelId.HasValue
                    ? properties.ChannelId.Value != null
                        ? properties.ChannelId.Value.Value.RawValue
                        : (ulong?) null
                    : Optional<ulong?>.Empty
            };
            return SendRequestAsync<WidgetModel>(new RestRequest(PATCH, $"guilds/{guildId:guild_id}/embed", requestContent, options));
        }

        public async Task<string> GetGuildVanityUrlAsync(ulong guildId, RestRequestOptions options)
        {
            var model = await SendRequestAsync<InviteModel>(new RestRequest(GET, $"guilds/{guildId:guild_id}/vanity-url", options)).ConfigureAwait(false);
            return model.Code;
        }

        // Invite
        public Task<InviteModel> GetInviteAsync(string code, bool? withCounts, RestRequestOptions options)
        {
            Dictionary<string, object> parameters = null;
            if (withCounts != null)
            {
                parameters = new Dictionary<string, object>
                {
                    ["with_counts"] = withCounts.Value
                };
            }
            return SendRequestAsync<InviteModel>(new RestRequest(GET, $"invites/{code}", parameters, options));
        }

        public Task<InviteModel> DeleteInviteAsync(string code, RestRequestOptions options)
            => SendRequestAsync<InviteModel>(new RestRequest(DELETE, $"invites/{code}", options));

        // User
        public Task<UserModel> GetCurrentUserAsync(RestRequestOptions options)
            => SendRequestAsync<UserModel>(new RestRequest(GET, $"users/@me", options));

        public Task<UserModel> GetUserAsync(ulong userId, RestRequestOptions options)
            => SendRequestAsync<UserModel>(new RestRequest(GET, $"users/{userId}", options));

        public Task<UserModel> ModifyCurrentUserAsync(ModifyCurrentUserProperties properties, RestRequestOptions options)
        {
            if (TokenType == TokenType.User)
            {
                if ((properties.Name.HasValue || properties.Password.HasValue || properties.Discriminator.HasValue)
                    && options.Password == null)
                    throw new ArgumentException("The password is required to change the name, discriminator, or to set a new password.");
            }

            var requestContent = new ModifyCurrentUserContent
            {
                Username = properties.Name,
                Avatar = properties.Avatar,
                NewPassword = properties.Password,
                Discriminator = properties.Discriminator
            };
            return SendRequestAsync<UserModel>(new RestRequest(PATCH, $"users/@me", requestContent, options));
        }

        public Task LeaveGuildAsync(ulong guildId, RestRequestOptions options)
            => SendRequestAsync(new RestRequest(DELETE, $"users/@me/guilds/{guildId:guild_id}", options));

        public Task<ChannelModel> CreateDmAsync(ulong recipientId, RestRequestOptions options)
        {
            var requestContent = new CreateDmContent
            {
                RecipientId = recipientId
            };
            return SendRequestAsync<ChannelModel>(new RestRequest(POST, $"users/@me/channels", requestContent, options));
        }

        // Voice
        public Task<VoiceRegionModel[]> ListVoiceRegionsAsync(RestRequestOptions options)
            => SendRequestAsync<VoiceRegionModel[]>(new RestRequest(GET, $"voice/regions", options));

        // Aaaa

        public Task<GatewayModel> GetGatewayAsync(RestRequestOptions options)
            => SendRequestAsync<GatewayModel>(new RestRequest(GET, $"gateway", options));

        public Task<GatewayBotModel> GetGatewayBotAsync(RestRequestOptions options)
            => SendRequestAsync<GatewayBotModel>(new RestRequest(GET, $"gateway/bot", options));

        public Task<WebhookModel> CreateWebhookAsync(ulong channelId, string name, Stream avatar, RestRequestOptions options)
        {
            var requestContent = new CreateWebhookContent
            {
                Name = name,
                Avatar = avatar
            };
            return SendRequestAsync<WebhookModel>(new RestRequest(POST, $"channels/{channelId:channel_id}/webhooks", requestContent, options));
        }

        public Task<WebhookModel[]> GetChannelWebhooksAsync(ulong channelId, RestRequestOptions options)
            => SendRequestAsync<WebhookModel[]>(new RestRequest(GET, $"channels/{channelId:channel_id}/webhooks", options));

        public Task<WebhookModel[]> GetGuildWebhooksAsync(ulong guildId, RestRequestOptions options)
            => SendRequestAsync<WebhookModel[]>(new RestRequest(GET, $"guilds/{guildId:guild_id}/webhooks", options));

        public Task<WebhookModel> GetWebhookAsync(ulong webhookId, RestRequestOptions options)
            => SendRequestAsync<WebhookModel>(new RestRequest(GET, $"webhooks/{webhookId:webhook_id}", options));

        public Task<WebhookModel> GetWebhookWithTokenAsync(ulong webhookId, string webhookToken, RestRequestOptions options)
            => SendRequestAsync<WebhookModel>(new RestRequest(GET, $"webhooks/{webhookId:webhook_id}/{webhookToken}", options));

        public Task<WebhookModel> ModifyWebhookAsync(ulong webhookId, ModifyWebhookProperties properties, RestRequestOptions options)
        {
            var requestContent = new ModifyWebhookContent
            {
                Name = properties.Name,
                Avatar = properties.Avatar,
                ChannelId = properties.ChannelId.HasValue
                    ? properties.ChannelId.Value.RawValue
                    : Optional<ulong>.Empty
            };
            return SendRequestAsync<WebhookModel>(new RestRequest(PATCH, $"webhooks/{webhookId:webhook_id}", requestContent, options));
        }

        public Task<WebhookModel> ModifyWebhookWithTokenAsync(ulong webhookId, string webhookToken, ModifyWebhookProperties properties, RestRequestOptions options)
        {
            var requestContent = new ModifyWebhookContent
            {
                Name = properties.Name,
                Avatar = properties.Avatar
            };
            return SendRequestAsync<WebhookModel>(new RestRequest(PATCH, $"webhooks/{webhookId:webhook_id}/{webhookToken}", requestContent, options));
        }

        public Task DeleteWebhookAsync(ulong webhookId, RestRequestOptions options)
            => SendRequestAsync(new RestRequest(DELETE, $"webhooks/{webhookId:webhook_id}", options));

        public Task DeleteWebhookWithTokenAsync(ulong webhookId, string webhookToken, RestRequestOptions options)
            => SendRequestAsync(new RestRequest(DELETE, $"webhooks/{webhookId:webhook_id}/{webhookToken}", options));

        public Task<MessageModel> ExecuteWebhookAsync(ulong webhookId, string webhookToken,
            string content, bool isTTS, IEnumerable<LocalEmbed> embeds,
            string name, string avatarUrl,
            bool wait,
            RestRequestOptions options)
        {
            var requestContent = new ExecuteWebhookContent
            {
                Username = name,
                AvatarUrl = avatarUrl,
                Content = content,
                TTS = isTTS,
                Embeds = embeds?.Select(x => x.ToModel()).ToArray()
            };
            return SendRequestAsync<MessageModel>(new RestRequest(POST, $"webhooks/{webhookId:webhook_id}/{webhookToken}", new Dictionary<string, object>
            {
                ["wait"] = wait
            }, requestContent, options));
        }

        public Task<MessageModel> ExecuteWebhookAsync(ulong webhookId, string webhookToken,
            LocalAttachment attachment,
            string content, bool isTTS, IEnumerable<LocalEmbed> embeds,
            string name, string avatarUrl,
            bool wait,
            RestRequestOptions options)
        {
            var requestContent = new MultipartRequestContent<ExecuteWebhookContent>
            {
                Content = new ExecuteWebhookContent
                {
                    Username = name,
                    AvatarUrl = avatarUrl,
                    Content = content,
                    TTS = isTTS,
                    Embeds = embeds?.Select(x => x.ToModel()).ToArray()
                },
                Attachment = attachment
            };
            return SendRequestAsync<MessageModel>(new RestRequest(POST, $"webhooks/{webhookId:webhook_id}/{webhookToken}", new Dictionary<string, object>
            {
                ["wait"] = wait
            }, requestContent, options));
        }

        public Task<MessageModel> ExecuteWebhookAsync(ulong webhookId, string webhookToken,
            IEnumerable<LocalAttachment> attachments,
            string content, bool isTTS, IEnumerable<LocalEmbed> embeds,
            string name, string avatarUrl,
            bool wait,
            RestRequestOptions options)
        {
            var requestContent = new MultipartRequestContent<ExecuteWebhookContent>
            {
                Content = new ExecuteWebhookContent
                {
                    Username = name,
                    AvatarUrl = avatarUrl,
                    Content = content,
                    TTS = isTTS,
                    Embeds = embeds?.Select(x => x.ToModel()).ToArray()
                },
                Attachments = attachments.ToArray()
            };
            return SendRequestAsync<MessageModel>(new RestRequest(POST, $"webhooks/{webhookId:webhook_id}/{webhookToken}", new Dictionary<string, object>
            {
                ["wait"] = wait
            }, requestContent, options));
        }

        public Task<ApplicationModel> GetCurrentApplicationInformationAsync(RestRequestOptions options)
            => SendRequestAsync<ApplicationModel>(new RestRequest(GET, $"oauth2/applications/@me", options));

        public Task CreateRelationshipAsync(ulong userId, RelationshipType? type, RestRequestOptions options)
        {
            var requestContent = new CreateRelationshipContent
            {
                Type = type
            };

            return SendRequestAsync(new RestRequest(PUT, $"users/@me/relationships/{userId}", requestContent, options));
        }

        public Task DeleteRelationshipAsync(ulong userId, RestRequestOptions options)
            => SendRequestAsync(new RestRequest(DELETE, $"users/@me/relationships/{userId}", options));

        public Task SendFriendRequestAsync(string name, string discriminator, RestRequestOptions options)
        {
            var requestContent = new SendFriendRequestContent
            {
                Username = name,
                Discriminator = discriminator
            };

            return SendRequestAsync(new RestRequest(POST, $"users/@me/relationships", requestContent, options));
        }

        public Task<ProfileModel> GetUserProfileAsync(ulong userId, RestRequestOptions options)
            => SendRequestAsync<ProfileModel>(new RestRequest(GET, $"users/{userId}/profile", options));

        public Task<RelationshipModel[]> GetRelationshipsAsync(RestRequestOptions options)
            => SendRequestAsync<RelationshipModel[]>(new RestRequest(GET, $"users/@me/relationships", options));

        public Task<UserModel[]> GetMutualFriendsAsync(ulong userId, RestRequestOptions options)
            => SendRequestAsync<UserModel[]>(new RestRequest(GET, $"users/{userId}/relationships", options));

        public Task CreateNoteAsync(ulong userId, string note, RestRequestOptions options)
        {
            var requestContent = new CreateNoteContent
            {
                Note = note
            };

            return SendRequestAsync(new RestRequest(PUT, $"users/@me/notes/{userId}", requestContent, options));
        }

        public async Task<string> AckMessageAsync(ulong channelId, ulong messageId, string token, RestRequestOptions options)
        {
            var requestContent = new AckMessageContent
            {
                Token = token
            };
            var model = await SendRequestAsync<AckMessageContent>(new RestRequest(POST, $"channels/{channelId:channel_id}/messages/{messageId}/ack", requestContent, options));
            return model.Token;
        }

        public Task AcceptInviteAsync(string code, RestRequestOptions options)
            => SendRequestAsync(new RestRequest(POST, $"invites/{code}", options));

        public Task<UserSettingsModel> GetUserSettingsAsync(RestRequestOptions options)
            => SendRequestAsync<UserSettingsModel>(new RestRequest(GET, $"users/@me/settings", options));

        public Task<UserSettingsModel> ModifyUserSettingsAsync(ModifyUserSettingsProperties properties, RestRequestOptions options)
        {
            var model = new UserSettingsModel
            {
                TimezoneOffset = properties.TimezoneOffset,
                Theme = properties.Theme,
                StreamNotificationsEnabled = properties.EnableStreamNotifications,
                Status = properties.Status,
                ShowCurrentGame = properties.ShowCurrentGame,
                RestrictedGuilds = properties.RestrictedGuildIds.HasValue
                    ? properties.RestrictedGuildIds.Value.Select(x => x.RawValue).ToArray()
                    : Optional<ulong[]>.Empty,
                RenderReactions = properties.RenderReactions,
                RenderEmbeds = properties.RenderEmbeds,
                MessageDisplayCompact = properties.EnableCompactMessages,
                Locale = properties.Locale.HasValue
                    ? properties.Locale.Value.Name
                    : Optional<string>.Empty,
                InlineEmbedMedia = properties.ShowEmbeds,
                InlineAttachmentMedia = properties.ShowAttachments,
                GifAutoPlay = properties.AutomaticallyPlayGifs,
                FriendSourceFlags = properties.FriendSource.HasValue
                    ? properties.FriendSource.Value.ToModel()
                    : Optional<FriendSourceFlagsModel>.Empty,
                ExplicitContentFilter = properties.ContentFilterLevel,
                EnableTtsCommand = properties.EnableTts,
                DisableGamesTab = properties.DisableGamesTab,
                DeveloperMode = properties.EnableDeveloperMode,
                DetectPlatformAccounts = properties.DetectPlatformAccounts,
                DefaultGuildsRestricted = properties.RestrictGuildsByDefault,
                ConvertEmoticons = properties.ConvertEmojis,
                AnimateEmoji = properties.AnimateEmojis,
                AfkTimeout = properties.AfkTimeout.HasValue
                    ? (long) properties.AfkTimeout.Value.TotalSeconds
                    : Optional<long>.Empty
            };
            return SendRequestAsync<UserSettingsModel>(new RestRequest(PATCH, $"users/@me/settings", new JsonObjectContent(model), options));
        }

        public void Log(LogMessageSeverity severity, string message, Exception exception = null)
            => Logger.Log(this, new MessageLoggedEventArgs("Rest", severity, message, exception));

        public void Dispose()
        {
            Http.Dispose();
        }
    }
}
